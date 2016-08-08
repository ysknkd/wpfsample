# WPFでMVVM
WPFでMVVMパターンを採用するときの備忘録です。

## MVVMの設定
WPFアプリケーションのプロジェクトを作成した時点では、View（XAMLとコードビハインド）しかありません。
ViewModelを用意して、Viewと関連付けを行う必要があります。

### ViewModelの用意
WPF標準としてのViewModelの基底クラスはないので、ViewModelとして必要な機能を備えた抽象クラスを作成して、継承することとします。
抽象クラスは、`ViewModelBase`として作成します。`ViewModelBase`の内容は`WpfSample/Common/ViewModelBase.cs`を参照してください。

### XAMLへ宣言の追加
`ViewModelBase`を継承したクラスを用意した上で、次に View と ViewModel の関連付けを行うために、XAMLへ次の内容を追記します。

```xml
<Window x:Class="WpfSample.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfSample"
        xmlns:VM="clr-namespace:WpfSample.ViewModels"
        mc:Ignorable="d"
        Title="MainWindow" Height="354" Width="576">
    <!-- データバインディング -->
    <Window.DataContext>
        <VM:MainWindowViewModel />
    </Window.DataContext>
    ...
```

上記内容を追記すると、Viewが生成される際にViewModelも生成されるようになります。

## リソースの利用
WPFでリソースを利用する方法を記載します。
ここでのリソースは、XAMLのスタイルと、文字列のリソースを指します。

### リソースの定義方法
それぞれ以下を参照のこと。

 * XAMLのスタイル
   * `CommonResources/Themes/Generic.xaml`
 * 文字列リソース
   * `CommonResources/TextResources.xaml`

### 別のDLLからリソースを読み込んで、XAML内で利用する方法
リソースを使用したいViewのXAMLに以下の記述を追加します。

```xml
    <!-- リソースの読み込み -->
    <Window.Resources>
        <!-- 外部DLLからの読み込み -->
        <!-- @see http://garafu.blogspot.jp/2015/12/wpf-resouce-dll.html -->
        <ResourceDictionary>
            <!-- MergedDictionaries タグ内に記載しないと複数読み込めない -->
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/CommonResources;component/Themes/Generic.xaml" />
                <ResourceDictionary Source="/CommonResources;component/TextResources.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Window.Resources>
```

TextResourcesには、`LABEL_SEARCH`でラベルのリソースが定義されています。
次のように記述することで、外部から読み込んだリソースを使用できます。

```xml
        <Button x:Name="button"
    		Content="{StaticResource LABEL_SEARCH}"
    		HorizontalAlignment="Left" VerticalAlignment="Top" Height="30" Width="114" Margin="10,10,0,0"/>
```

### 別のDLLからリソースを読み込んで、コード内で利用する方法
XAMLで読み込んだリソースは、Viewには反映されていますが、ViewModelから直接参照する方法がありません。
仕方がないので、デリゲートを使ってViewから`FindResources`を拝借することにします。

次の内容をViewModelBase.csに記述します。

```cs
    public Func<string, object> FindResources;

    public string Resources(string key)
    {
        return (string)FindResources(key);
    }
```

少し心苦しいですが、コードビハインドに次の内容を記述します。

```cs
    var viewModel = (ViewModelBase)DataContext;
    viewModel.FindResources = FindResource
```

## バリデーションの実施
XAML側での入力制限とModelによるバリデーションを検討した結果を以下に記載します。

### XAMLによる入力制限
入力の桁数については、XAML側で簡単に実施することができます。
ただし、入力可能文字を限定するためにはロジックを書かなくてはなりません。
IMEの変更を制限することはできますが、コピペで入力される文字を制限することまではできません。

### Modelによるバリデーション
ViewModelからModelに対して操作を行った際に、Modelでバリデーションを実施することを検討してみました。
(ViewやViewModelでバリデーションを実施する記事はよくありますが、Modelで実施する記事はなかなか見つかりませんでした。)

`ArtistModel.cs` で、処理を実施する前にバリデーションを実施して、問題があればエラーを返却します。
エラーを返却する際に、問題のプロパティ名を一緒に渡しています。

```
    public async Task<TaskResult> Add()
    {
        if (string.IsNullOrEmpty(ArtistName))
        {
            return new TaskResult
            {
                result = TaskResultType.EREQUIRED, propertyName = nameof(ArtistName)
            };
        }

        await Task.Run(() =>
        {
            // 時間のかかる処理を再現
            Thread.Sleep(200);
        });
        Artists.Add(new Artist { Id = Artists.Count, Name = ArtistName });

        return new TaskResult
        {
            result = TaskResultType.SUCCEEDED
        };
    }
```

`MainWindowViewModel` では、実行結果を確認して、必要であればダイアログを表示します。
ダイアログで表示するメッセージは、リソースから取得したものです。

```cs
    private async void ExecuteAddArtist(object state)
    {
        TaskResult result = await _artists.Add();

        switch (result.result)
        {
            case TaskResultType.SUCCEEDED:
                MessageBox.Show(ArtistName + Resources("MSG_DIALOG_ADDED"), Resources("MSG_DIALOG_TITLE_CONFIRM"));
                break;
            case TaskResultType.EREQUIRED:
                if (result.propertyName == nameof(ArtistName))
                {
                    MessageBox.Show(Resources("MSG_DIALOG_REQUIRE"), Resources("MSG_DIALOG_TITLE_ERROR"));
                }
                break;
            default:
                break;
        }
    }
```

上記の設計で問題なのは（なりそうなのは）...

 1. 入力した直後にバリデーションを実施できないこと。
 2. 問題のプロパティを複数通知できないこと。

くらいでしょうか。
「2.」に関しては、結果のクラス（`TaskResult`）に手を入れればできそうです。

メリットとしては、Model でバリデーションを実施することで、Presentationに依存しないテストが実施できることでしょうか。

---
以上