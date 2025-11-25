using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MultiAgentLearning.UI.Models;

namespace MultiAgentLearning.UI.Converters;

/// <summary>
/// Converts AgentStatus to a color brush for visual indication
/// </summary>
public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AgentStatus status)
        {
            return status switch
            {
                AgentStatus.NotStarted => new SolidColorBrush(Color.FromRgb(128, 128, 128)), // Gray
                AgentStatus.Processing => new SolidColorBrush(Color.FromRgb(255, 165, 0)),   // Orange
                AgentStatus.Completed => new SolidColorBrush(Color.FromRgb(40, 167, 69)),    // Green
                AgentStatus.Error => new SolidColorBrush(Color.FromRgb(220, 53, 69)),        // Red
                _ => new SolidColorBrush(Colors.Gray)
            };
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
