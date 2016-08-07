# WPFでMVVM
WPFでMVVMパターンを採用するときの備忘録です。

## MVVMの設定
WPFアプリケーションのプロジェクトを作成した時点では、View（XAMLとコードビハインド）しかありません。
ViewModelを用意して、Viewと関連付けを行う必要があります。

### ViewModelの用意
WPFの標準で特にViewModelとしての基底クラスはないので、ViewModelとして必要な機能を備えた抽象クラスを作成して、継承することとします。
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

上記内容を追記することで、Viewが生成される際にViewModelも生成されるようになります。

## リソースの利用
WPFでリソースを利用する方法を記載します。

### リソースの定義方法
`CommonResources/Themes/Generic.xaml`および`CommonResources/TextResources.xaml`を参照のこと。

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