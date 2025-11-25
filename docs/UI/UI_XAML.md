# XAML Layout Reference

## MainWindow.xaml - Complete Implementation

This document contains the complete XAML markup for the main window of the Multi-Agent Learning System.

```xml
<Window x:Class="MultiAgentLearning.UI.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:MultiAgentLearning.UI.Views"
        xmlns:vm="clr-namespace:MultiAgentLearning.UI.ViewModels"
        xmlns:converters="clr-namespace:MultiAgentLearning.UI.Converters"
        mc:Ignorable="d"
        d:DataContext="{d:DesignInstance Type=vm:MainViewModel}"
        Title="Multi-Agent Learning System" 
        Height="800" 
        Width="1000"
        MinHeight="600"
        MinWidth="800"
        WindowStartupLocation="CenterScreen"
        Background="{StaticResource PrimaryBackgroundBrush}">

    <Window.Resources>
        <!-- Merge application styles -->
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/Resources/Styles/AppStyles.xaml"/>
            </ResourceDictionary.MergedDictionaries>

            <!-- Converters -->
            <converters:StatusToColorConverter x:Key="StatusToColorConverter"/>
            <converters:BoolToVisibilityConverter x:Key="BoolToVisibilityConverter"/>
            <converters:InverseBoolConverter x:Key="InverseBoolConverter"/>
        </ResourceDictionary>
    </Window.Resources>

    <Grid Margin="16">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>  <!-- Orchestration Pattern -->
            <RowDefinition Height="Auto"/>  <!-- Question Input -->
            <RowDefinition Height="Auto"/>  <!-- Agent Processing -->
            <RowDefinition Height="*"/>     <!-- Results -->
            <RowDefinition Height="Auto"/>  <!-- Status Bar -->
        </Grid.RowDefinitions>

        <!-- Orchestration Pattern Selector -->
        <GroupBox Grid.Row="0" Header="Orchestration Pattern">
            <StackPanel Orientation="Horizontal" Spacing="20">
                <RadioButton Content="Sequential" 
                           IsChecked="{Binding SelectedOrchestrationMode, 
                                      Converter={StaticResource EnumToBoolConverter}, 
                                      ConverterParameter=Sequential}"
                           IsEnabled="{Binding IsProcessing, 
                                      Converter={StaticResource InverseBoolConverter}}"
                           FontSize="14"
                           VerticalAlignment="Center"/>
                
                <RadioButton Content="Concurrent" 
                           IsChecked="{Binding SelectedOrchestrationMode, 
                                      Converter={StaticResource EnumToBoolConverter}, 
                                      ConverterParameter=Concurrent}"
                           IsEnabled="{Binding IsProcessing, 
                                      Converter={StaticResource InverseBoolConverter}}"
                           FontSize="14"
                           VerticalAlignment="Center"
                           Margin="20,0,0,0"/>
            </StackPanel>
        </GroupBox>

        <!-- Question Input Area -->
        <GroupBox Grid.Row="1" Header="Your Question">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="*"/>
                    <RowDefinition Height="Auto"/>
                </Grid.RowDefinitions>

                <!-- Multi-line TextBox -->
                <TextBox Grid.Row="0"
                       Text="{Binding QuestionText, UpdateSourceTrigger=PropertyChanged}"
                       AcceptsReturn="True"
                       TextWrapping="Wrap"
                       VerticalScrollBarVisibility="Auto"
                       Height="100"
                       IsEnabled="{Binding IsProcessing, 
                                  Converter={StaticResource InverseBoolConverter}}">
                    <TextBox.Style>
                        <Style TargetType="TextBox" BasedOn="{StaticResource {x:Type TextBox}}">
                            <Style.Triggers>
                                <Trigger Property="Text" Value="">
                                    <Setter Property="Background">
                                        <Setter.Value>
                                            <VisualBrush Stretch="None" AlignmentX="Left" AlignmentY="Top">
                                                <VisualBrush.Visual>
                                                    <TextBlock Text="Enter your question or prompt..." 
                                                             Foreground="{StaticResource TextSecondaryBrush}"
                                                             FontStyle="Italic"
                                                             Margin="8,4,0,0"/>
                                                </VisualBrush.Visual>
                                            </VisualBrush>
                                        </Setter.Value>
                                    </Setter>
                                </Trigger>
                            </Style.Triggers>
                        </Style>
                    </TextBox.Style>
                </TextBox>

                <!-- Ask Button -->
                <Button Grid.Row="1"
                      Content="{Binding IsProcessing, 
                               Converter={StaticResource ProcessingTextConverter}}"
                      Command="{Binding AskQuestionCommand}"
                      HorizontalAlignment="Right"
                      Margin="0,8,0,0"
                      MinWidth="150">
                    <Button.Style>
                        <Style TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
                            <Setter Property="Content" Value="Ask Question"/>
                            <Style.Triggers>
                                <DataTrigger Binding="{Binding IsProcessing}" Value="True">
                                    <Setter Property="Content" Value="Processing..."/>
                                </DataTrigger>
                            </Style.Triggers>
                        </Style>
                    </Button.Style>
                </Button>
            </Grid>
        </GroupBox>

        <!-- Agent Processing Panel -->
        <GroupBox Grid.Row="2" Header="Agent Processing">
            <ItemsControl ItemsSource="{Binding AgentStatuses}">
                <ItemsControl.ItemTemplate>
                    <DataTemplate>
                        <Grid Margin="0,0,0,12">
                            <Grid.RowDefinitions>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="*"/>
                                <ColumnDefinition Width="Auto"/>
                            </Grid.ColumnDefinitions>

                            <!-- Agent Name and Status -->
                            <StackPanel Grid.Row="0" Grid.Column="0" 
                                      Orientation="Horizontal">
                                <Ellipse Width="10" Height="10" 
                                       Fill="{Binding Status, 
                                             Converter={StaticResource StatusToColorConverter}}"
                                       VerticalAlignment="Center"
                                       Margin="0,0,8,0"/>
                                <TextBlock Text="{Binding AgentName}" 
                                         FontWeight="SemiBold"
                                         FontSize="14"
                                         VerticalAlignment="Center"/>
                            </StackPanel>

                            <!-- Status Text -->
                            <TextBlock Grid.Row="0" Grid.Column="1"
                                     Text="{Binding StatusText}"
                                     Foreground="{StaticResource TextSecondaryBrush}"
                                     FontSize="12"
                                     VerticalAlignment="Center"
                                     Margin="8,0,0,0"/>

                            <!-- Progress Bar -->
                            <ProgressBar Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2"
                                       Value="{Binding Progress}"
                                       Maximum="100"
                                       Margin="0,4,0,0">
                                <ProgressBar.Style>
                                    <Style TargetType="ProgressBar" BasedOn="{StaticResource {x:Type ProgressBar}}">
                                        <Style.Triggers>
                                            <!-- Indeterminate when processing -->
                                            <DataTrigger Binding="{Binding Status}" 
                                                       Value="{x:Static local:AgentStatus.Processing}">
                                                <Setter Property="IsIndeterminate" Value="True"/>
                                            </DataTrigger>
                                            <!-- Green when completed -->
                                            <DataTrigger Binding="{Binding Status}" 
                                                       Value="{x:Static local:AgentStatus.Completed}">
                                                <Setter Property="Foreground" 
                                                      Value="{StaticResource SuccessBrush}"/>
                                            </DataTrigger>
                                            <!-- Red when error -->
                                            <DataTrigger Binding="{Binding Status}" 
                                                       Value="{x:Static local:AgentStatus.Error}">
                                                <Setter Property="Foreground" 
                                                      Value="{StaticResource ErrorBrush}"/>
                                            </DataTrigger>
                                        </Style.Triggers>
                                    </Style>
                                </ProgressBar.Style>
                            </ProgressBar>
                        </Grid>
                    </DataTemplate>
                </ItemsControl.ItemTemplate>
            </ItemsControl>
        </GroupBox>

        <!-- Results Display Area -->
        <GroupBox Grid.Row="3" Header="Results" Margin="0,0,0,8">
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="*"/>
                    <RowDefinition Height="Auto"/>
                </Grid.RowDefinitions>

                <!-- Results TextBox -->
                <Border Grid.Row="0" 
                      BorderBrush="{StaticResource BorderBrush}"
                      BorderThickness="1"
                      Background="{StaticResource SecondaryBackgroundBrush}"
                      Padding="8">
                    <ScrollViewer VerticalScrollBarVisibility="Auto">
                        <TextBox Text="{Binding ResultText, Mode=OneWay}"
                               IsReadOnly="True"
                               TextWrapping="Wrap"
                               BorderThickness="0"
                               Background="Transparent"
                               FontFamily="Consolas"
                               FontSize="12"/>
                    </ScrollViewer>
                </Border>

                <!-- Action Buttons -->
                <StackPanel Grid.Row="1" 
                          Orientation="Horizontal" 
                          HorizontalAlignment="Right"
                          Margin="0,8,0,0">
                    <Button Content="Copy to Clipboard"
                          Command="{Binding CopyResultsCommand}"
                          Margin="0,0,8,0"
                          Padding="12,6">
                        <Button.Style>
                            <Style TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
                                <Setter Property="Background" Value="{StaticResource SecondaryBackgroundBrush}"/>
                                <Setter Property="Foreground" Value="{StaticResource TextPrimaryBrush}"/>
                            </Style>
                        </Button.Style>
                    </Button>

                    <Button Content="Export to File"
                          Command="{Binding ExportResultsCommand}"
                          Margin="0,0,8,0"
                          Padding="12,6">
                        <Button.Style>
                            <Style TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
                                <Setter Property="Background" Value="{StaticResource SecondaryBackgroundBrush}"/>
                                <Setter Property="Foreground" Value="{StaticResource TextPrimaryBrush}"/>
                            </Style>
                        </Button.Style>
                    </Button>

                    <Button Content="Clear"
                          Command="{Binding ClearResultsCommand}"
                          Padding="12,6">
                        <Button.Style>
                            <Style TargetType="Button" BasedOn="{StaticResource {x:Type Button}}">
                                <Setter Property="Background" Value="{StaticResource SecondaryBackgroundBrush}"/>
                                <Setter Property="Foreground" Value="{StaticResource TextPrimaryBrush}"/>
                            </Style>
                        </Button.Style>
                    </Button>
                </StackPanel>
            </Grid>
        </GroupBox>

        <!-- Status Bar -->
        <StatusBar Grid.Row="4">
            <StatusBarItem>
                <StackPanel Orientation="Horizontal">
                    <TextBlock Text="Status: " 
                             Foreground="{StaticResource TextSecondaryBrush}"/>
                    <TextBlock Text="{Binding StatusMessage}" 
                             FontWeight="SemiBold"/>
                </StackPanel>
            </StatusBarItem>

            <StatusBarItem HorizontalAlignment="Right">
                <StackPanel Orientation="Horizontal">
                    <Ellipse Width="8" Height="8" 
                           Fill="{StaticResource SuccessBrush}"
                           VerticalAlignment="Center"
                           Margin="0,0,4,0"/>
                    <TextBlock Text="API Connected" 
                             Foreground="{StaticResource TextSecondaryBrush}"
                             FontSize="11"/>
                </StackPanel>
            </StatusBarItem>
        </StatusBar>
    </Grid>
</Window>
```

## Alternative Compact Layout

For smaller screens, consider this more compact version:

```xml
<!-- Compact version with TabControl for results -->
<Window x:Class="MultiAgentLearning.UI.Views.MainWindow"
        ...
        Height="700" Width="900">
    
    <Grid Margin="12">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>  <!-- Settings -->
            <RowDefinition Height="Auto"/>  <!-- Input -->
            <RowDefinition Height="*"/>     <!-- Tabbed Content -->
            <RowDefinition Height="Auto"/>  <!-- Status -->
        </Grid.RowDefinitions>

        <!-- Combined Settings -->
        <StackPanel Grid.Row="0" Margin="0,0,0,8">
            <TextBlock Text="Orchestration:" 
                     FontWeight="SemiBold" 
                     Margin="0,0,0,4"/>
            <StackPanel Orientation="Horizontal">
                <RadioButton Content="Sequential" 
                           IsChecked="{Binding IsSequential}"
                           Margin="0,0,20,0"/>
                <RadioButton Content="Concurrent" 
                           IsChecked="{Binding IsConcurrent}"/>
            </StackPanel>
        </StackPanel>

        <!-- Question Input -->
        <GroupBox Grid.Row="1" Header="Question" Padding="8">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>

                <TextBox Grid.Column="0" 
                       Text="{Binding QuestionText}"
                       Height="60"
                       TextWrapping="Wrap"
                       VerticalScrollBarVisibility="Auto"/>
                
                <Button Grid.Column="1" 
                      Content="Ask"
                      Command="{Binding AskQuestionCommand}"
                      Margin="8,0,0,0"
                      Width="80"/>
            </Grid>
        </GroupBox>

        <!-- Tabbed Content -->
        <TabControl Grid.Row="2" Margin="0,8">
            <TabItem Header="Results">
                <!-- Results content -->
            </TabItem>
            <TabItem Header="Agents">
                <!-- Agent status content -->
            </TabItem>
            <TabItem Header="History">
                <!-- Query history -->
            </TabItem>
        </TabControl>

        <!-- Status Bar -->
        <StatusBar Grid.Row="3"/>
    </Grid>
</Window>
```

## Custom Control: AgentStatusControl.xaml

Optional custom control for agent status display:

```xml
<UserControl x:Class="MultiAgentLearning.UI.Views.Controls.AgentStatusControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:converters="clr-namespace:MultiAgentLearning.UI.Converters"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             mc:Ignorable="d">

    <UserControl.Resources>
        <converters:StatusToColorConverter x:Key="StatusToColorConverter"/>
    </UserControl.Resources>

    <Border BorderBrush="{StaticResource BorderBrush}"
            BorderThickness="1"
            CornerRadius="4"
            Padding="12"
            Margin="0,0,0,8">
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>

            <!-- Header -->
            <Grid Grid.Row="0">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>

                <StackPanel Orientation="Horizontal">
                    <Ellipse Width="12" Height="12"
                           Fill="{Binding Status, 
                                 Converter={StaticResource StatusToColorConverter}}"
                           VerticalAlignment="Center"
                           Margin="0,0,8,0"/>
                    <TextBlock Text="{Binding AgentName}"
                             FontSize="16"
                             FontWeight="SemiBold"
                             VerticalAlignment="Center"/>
                </StackPanel>

                <TextBlock Grid.Column="1"
                         Text="{Binding StatusText}"
                         Foreground="{StaticResource TextSecondaryBrush}"
                         VerticalAlignment="Center"/>
            </Grid>

            <!-- Progress Bar -->
            <ProgressBar Grid.Row="1"
                       Value="{Binding Progress}"
                       Maximum="100"
                       Height="6"
                       Margin="0,8,0,0"
                       Foreground="{Binding Status, 
                                   Converter={StaticResource StatusToColorConverter}}"/>

            <!-- Details (Optional) -->
            <StackPanel Grid.Row="2" 
                      Margin="0,8,0,0"
                      Visibility="{Binding ShowDetails, 
                                  Converter={StaticResource BoolToVisibilityConverter}}">
                <TextBlock Text="{Binding ElapsedTime, StringFormat='Time: {0:mm\\:ss}'}"
                         FontSize="12"
                         Foreground="{StaticResource TextSecondaryBrush}"/>
                <TextBlock Text="{Binding Message}"
                         FontSize="12"
                         TextWrapping="Wrap"
                         Foreground="{StaticResource TextSecondaryBrush}"
                         Margin="0,4,0,0"/>
            </StackPanel>
        </Grid>
    </Border>
</UserControl>
```

## Additional Value Converter Needed

**File: `Converters/EnumToBoolConverter.cs`**

```csharp
using System.Globalization;
using System.Windows.Data;

namespace MultiAgentLearning.UI.Converters;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        return value.ToString() == parameter.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue && parameter != null)
        {
            return Enum.Parse(targetType, parameter.ToString()!);
        }

        return Binding.DoNothing;
    }
}
```

## Styling Notes

### Rounded Corners (Modern Look)

Add to AppStyles.xaml for modern rounded appearance:

```xml
<Style x:Key="RoundedGroupBox" TargetType="GroupBox">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="GroupBox">
                <Grid>
                    <Border BorderBrush="{TemplateBinding BorderBrush}"
                          BorderThickness="{TemplateBinding BorderThickness}"
                          Background="{TemplateBinding Background}"
                          CornerRadius="8"
                          Padding="{TemplateBinding Padding}">
                        <ContentPresenter/>
                    </Border>
                </Grid>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

### Button Hover Effects

Enhanced button style with animation:

```xml
<Style x:Key="ModernButton" TargetType="Button">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Border x:Name="border"
                      Background="{TemplateBinding Background}"
                      BorderBrush="{TemplateBinding BorderBrush}"
                      BorderThickness="{TemplateBinding BorderThickness}"
                      CornerRadius="4"
                      Padding="{TemplateBinding Padding}">
                    <ContentPresenter HorizontalAlignment="Center"
                                    VerticalAlignment="Center"/>
                </Border>
                <ControlTemplate.Triggers>
                    <Trigger Property="IsMouseOver" Value="True">
                        <Setter TargetName="border" Property="Background" 
                              Value="{StaticResource AccentBrush}"/>
                        <Setter TargetName="border" Property="Opacity" Value="0.9"/>
                    </Trigger>
                    <Trigger Property="IsPressed" Value="True">
                        <Setter TargetName="border" Property="Opacity" Value="0.7"/>
                    </Trigger>
                </ControlTemplate.Triggers>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

## Responsive Design Tips

1. **Use Grid with Star Sizing**: `Height="*"` allows flexible sizing
2. **MinHeight/MinWidth**: Prevent content from being cut off
3. **ScrollViewer**: Wrap content that may overflow
4. **TextWrapping**: Enable for TextBlocks with variable content
5. **Viewbox**: Scale content proportionally (use sparingly)

## Accessibility Considerations

Add to XAML for better accessibility:

```xml
<!-- Add AutomationProperties for screen readers -->
<Button Content="Ask Question"
        AutomationProperties.Name="Ask Question Button"
        AutomationProperties.HelpText="Submit your question to the AI agents"/>

<TextBox AutomationProperties.LabeledBy="{Binding ElementName=QuestionLabel}"/>
```

## Testing the Layout

Use Visual Studio's XAML Designer features:
- **Live Visual Tree**: Inspect element hierarchy
- **Live Property Explorer**: Modify properties in real-time
- **Hot Reload**: See changes instantly without rebuild

## Performance Optimization

1. **Virtualization**: For large ItemsControls
2. **x:Static**: Use instead of DynamicResource when possible
3. **Freeze Brushes**: Improve rendering performance
4. **Avoid Deep Nesting**: Keep visual tree shallow

```xml
<!-- Freeze brushes for better performance -->
<SolidColorBrush x:Key="FrozenBrush" Color="#007BFF" po:Freeze="True"/>
```
