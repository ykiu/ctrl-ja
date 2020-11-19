# ctrl-ja

左右の <kbd>ctrl</kbd> キーを単体で押したときに IME のオン/オフを切り替えるようにする PowerShell スクリプトです。左 <kbd>ctrl</kbd> キーで IME をオフ、右 <kbd>ctrl</kbd> キーで IME をオンにします。

## 対応環境

Windows 10でテストしていますが、PowerShell がインストールされた Windows 環境であれば動くと思います。

## 実行方法

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

## タスクスケジューラに登録する

このプログラムを常用する場合は、Windows のタスクスケジューラを使ってコンピューターの起動時に立ち上がるようにしておくと便利です。以下の手順に従って本プログラムをタスクスケジューラに登録します。

### 1. PowerShell を管理者権限で起動する

### 2. このリポジトリのルートに移動する

```powershell
cd .\ctrl-ja\
```

### 3. 以下のスクリプトを貼り付ける

```powershell
$trigger = New-ScheduledTaskTrigger -AtLogon -User $env:USERNAME
$action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument ('-NoLogo -WindowStyle Hidden -Command "& {Add-Type -TypeDefinition (gc ' + (Resolve-Path .\ctrl-ja.cs) + ' -Raw) -ReferencedAssemblies System.Windows.Forms; [CtrlJa.Program]::Main();}"') -Id "ctrl-ja"
$settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -ExecutionTimeLimit (New-TimeSpan -Days 90)
Register-ScheduledTask -TaskName "ctrl-ja" -Trigger $trigger -Action $action -Settings $settings -User $env:USERNAME
```

### (別法) タスクスケジューラの GUI から登録する

タスクスケジューラの GUI からもプログラムを登録することができます。

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
        - 引数の追加: `-NoLogo -WindowStyle Hidden -Command "& {Add-Type -TypeDefinition (gc ctrl-ja.cs へのパス -Raw) -ReferencedAssemblies System.Windows.Forms; [CtrlJa.Program]::Main();}"`
            - たとえば、`ctrl-ja.cs` を C:\ 直下に置いた場合は `-NoLogo -WindowStyle Hidden -Command "& {Add-Type -TypeDefinition (gc C:\ctrl-ja.cs -Raw) -ReferencedAssemblies System.Windows.Forms; [CtrlJa.Program]::Main();}"` となります。
- 設定タブ
    - タスクを停止するまでの時間: チェックを外す

これで設定完了です。メニューの「操作」→「実行」を選択し、正しく実行されるか確かめてみてください。

## 謝辞

このプログラムは mac アプリ「[英かな](https://ei-kana.appspot.com/)」に触発されて作成しました。

PowerShell と C# でキー入力を監視する方法については次の記事を参考にしました。このリポジトリのコードの骨格部分には、リンク先のコードをほぼそのまま使っています。
[Creating a Key Logger via a Global System Hook using PowerShell](https://hinchley.net/articles/creating-a-key-logger-via-a-global-system-hook-using-powershell/)

また、Win32 API で IME を制御する方法については次の記事を参考にしました。この記事は AutoHotKey 用のものですが、Win32 の IME 回りの API の使用方法は、PowerShell + C# にも通用するものでした。
[(AutoHotkey)(IMEの制御をする 編) - もらかなです。](https://morakana.hatenadiary.org/entry/20080213/1202876561)

以上のプログラムおよび記事の作者に感謝申し上げます。
