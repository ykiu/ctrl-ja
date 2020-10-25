# ctrl-ja

左右の <kbd>ctrl</kbd> キーを単体で押したときに IME のオン/オフを切り替えるようにする PowerShell スクリプトです。左 <kbd>ctrl</kbd> キーで IME をオフ、右 <kbd>ctrl</kbd> キーで IME をオンにします。

## 対応環境

Windows 10でテストしていますが、PowerShell がインストールされた Windows 環境であれば動くと思います。

## 使い方 (お試し)

インストールしなくても使用感を確かめることができます。

### 1. PowerShell を起動する

### 2. このリポジトリをダウンロードし、リポジトリのルートに移動する

```powershell
git clone https://github.com/ykiu/ctrl-ja.git
cd .\ctrl-ja\
```

### 3. PowerShell の ExecutionPolicy の設定を緩める

現在開いている PowerShell 上で無署名のスクリプトを実行できるようにします。

```powershell
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope Process
```

### 4. 実行する

以下のスクリプトを実行すると、<kbd>ctrl</kbd> キーで IME の切り替えができるようになります。

```powershell
.\ctrl-ja.ps1
```

スクリプトを停止するには <kbd>ctrl+C</kbd> を押します。スクリプトを停止すると <kbd>ctrl</kbd> キーの働きは元通りになります。

## 使い方 (常用する場合)

このプログラムを常用する場合は、Windows のタスクスケジューラを使ってコンピューターの起動時に立ち上がるようにしておくと便利です。

そのためには、まずスクリプトに署名する必要があります。

### 署名する

署名をするには証明書が必要です。まず、証明書の名前 ("local"など) とパスワードを決めてください。次に、以下のスクリプトを PowerShell にペーストして証明書を作成してください。

```powershell
$Credential = Get-Credential -Message "証明書の名前 (local など) とパスワードを入力してください。"
$CredentialName = $Credential.UserName
$Password = $Credential.Password
$OutputPFXPath  = "$CredentialName.pfx"
$certificate = New-SelfSignedCertificate -subject $CredentialName -Type CodeSigning -CertStoreLocation "cert:\CurrentUser\My"
$pfxCertificate = Export-PfxCertificate $certificate -FilePath $OutputPFXPath -password $Password
Import-PfxCertificate $pfxCertificate -CertStoreLocation cert:\CurrentUser\Root -Password $Password
```

以上のスクリプトを実行すると .pfx ファイルが作成されます。.pfx ファイルはセキュアな場所に保管してください。

次に以下のスクリプトを PowerShell にペーストして、署名されたスクリプトを生成します。

```powershell
Copy-Item .\ctrl-ja.ps1 .\ctrl-ja.signed.ps1
Set-AuthenticodeSignature .\ctrl-ja.signed.ps1 -Certificate $certificate
```

これで署名は完了です。

### 署名済みのスクリプトを信頼する

次に、署名されたスクリプトを一度実行し、発行元を常に信頼するよう設定してください。

```powershell
.\ctrl-ja.signed.ps1

この信頼されていない発行元からのソフトウェアを実行しますか?
ファイル C:\Users\******\ctrl-ja\ctrl-ja.signed.ps1 の発行元は CN=local
であり、このシステムで信頼されていません。信頼された発行元からのスクリプトのみを実行してください。
[V] 常に実行しない(V)  [D] 実行しない(D)  [R] 一度だけ実行する(R)  [A] 常に実行する(A)  [?] ヘルプ (既定値は "D"): A
```

### タスクスケジューラに登録する

最後に、タスクスケジューラを使い、コンピューターの起動と同時にスクリプトが開始されるようにします。

「ファイル名を指定して実行」に「taskschd.msc」と入力し、タスクスケジューラを開きます。

左側のパネルからタスク スケジューラ (ローカル) → タスク スケジューラ ライブラリを選択します。

メニューの「操作」→「タスクの作成」から、タスクの作成ウィンドウを開きます。以下に従って設定し、OKをクリックします。記載のない項目はデフォルト値のままで問題ありません。

- 全般タブ
    - 名前: `(お好みの名前)`
- トリガータブ
    - 「新規」ボタンをクリック
        - タスクの開始: `ログオン時`
- 操作タブ
    - 「新規」ボタンをクリック
        - プログラム/スクリプト: `powershell.exe`
        - 引数の追加: `-NoLogo -WindowStyle Hidden -File (ctrl-ja.signed.ps1 へのパス)`

`-NoLogo -WindowStyle Hidden` の部分は、バックグラウンドで実行されるようにするために必要です。

これで設定完了です。メニューの「操作」→「実行」を選択し、正しく実行されるか確かめてみてください。

## 謝辞

このプログラムは mac アプリ「[英かな](https://ei-kana.appspot.com/)」に触発されて作成しました。

PowerShell と C# でキー入力を監視する方法については次の記事を参考にしました。このリポジトリのコードの骨格部分には、リンク先のコードをほぼそのまま使っています。
[Creating a Key Logger via a Global System Hook using PowerShell](https://hinchley.net/articles/creating-a-key-logger-via-a-global-system-hook-using-powershell/)

また、Win32 API で IME を制御する方法については次の記事を参考にしました。この記事は AutoHotKey 用のものですが、Win32 の IME 回りの API の使用方法は、PowerShell + C# にも通用するものでした。
[(AutoHotkey)(IMEの制御をする 編) - もらかなです。](https://morakana.hatenadiary.org/entry/20080213/1202876561)

以上のプログラムおよび記事の作者に感謝申し上げます。
