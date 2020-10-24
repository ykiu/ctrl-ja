# ctrl-ja

左右の <kbd>ctrl</kbd> キーを単体で押したときに IME のオン/オフを切り替えるようにする PowerShell スクリプトです。左 <kbd>ctrl</kbd> キーで IME をオフ、右 <kbd>ctrl</kbd> キーで IME をオンにします。

## 使い方 (お試し)

インストールせずに使用感を確かめることができます。

### 1. PowerShell を起動する

### 2. このリポジトリをダウンロードし、リポジトリのルートに移動する

```powershell
$ git clone https://github.com/ykiu/ctrl-ja.git
$ cd .\ctrl-ja\
```

### 3. PowerShell の ExecutionPolicy の設定を緩める

無署名のスクリプトを実行できるようにします。この設定の影響範囲は、以下のコマンドを打ち込んだ PowerShell セッションのみです。他の PowerShell セッションには影響しません。したがって本リポジトリのコードを信頼する限りにおいて、以下の設定は安全です。

```powershell
$ Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope Process
```

### 4. 実行する

以下のスクリプトを実行すると、<kbd>ctrl</kbd> キーで IME の切り替えができるようになります。

```powershell
$ .\ctrl-ja.ps1
```

スクリプトを停止するには <kbd>ctrl+C</kbd> を押します。スクリプトを停止すると <kbd>ctrl</kbd> キーの働きは元通りになります。

## 使い方 (常駐させる)

Windows のタスクスケジューラを使うと、コンピューターの起動時に ctrl-ja.ps を起動するように設定できます。

## 謝辞

このプログラムは mac アプリ「[英かな](https://ei-kana.appspot.com/)」に触発されて作成しました。

PowerShell と C# でキー入力を監視する方法については次の記事を参考にしました。このリポジトリのコードの骨格部分には、リンク先のコードをほぼそのまま使っています。
[Creating a Key Logger via a Global System Hook using PowerShell](https://hinchley.net/articles/creating-a-key-logger-via-a-global-system-hook-using-powershell/)

また、Win32 API で IME を制御する方法については次の記事を参考にしました。この記事は AutoHotKey 用のものですが、Win32 の IME 回りの API の使用方法は、PowerShell + C# にも通用するものでした。
[(AutoHotkey)(IMEの制御をする 編) - もらかなです。](https://morakana.hatenadiary.org/entry/20080213/1202876561)

以上のプログラムおよび記事の作者に感謝申し上げます。
