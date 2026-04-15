using System.Drawing;
using System.Windows.Forms;
using AppLog.Models;

namespace AppLog.Helpers;

/// <summary>
/// Helper class for managing log display in a RichTextBox.
/// Handles color-coded display, buffer management, and auto-scroll.
/// </summary>
public static class LogDisplayHelper
{
    /// <summary>
    /// Maximum log display buffer size in characters (1MB).
    /// </summary>
    private const int MaxBufferSize = 1048576;

    /// <summary>
    /// Color for timestamp text.
    /// </summary>
    private static readonly Color TimestampColor = Color.Gray;

    /// <summary>
    /// Color for TX (Transmit) direction text.
    /// </summary>
    private static readonly Color TxColor = Color.Blue;

    /// <summary>
    /// Color for RX (Receive) direction text.
    /// </summary>
    private static readonly Color RxColor = Color.Green;

    /// <summary>
    /// Appends a log entry to the RichTextBox with color coding.
    /// Manages buffer size by removing old entries when exceeding 1MB.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to append to.</param>
    /// <param name="logEntry">The log entry to display.</param>
    public static void AppendLogEntry(RichTextBox richTextBox, LogEntry logEntry)
    {
        if (richTextBox == null)
        {
            throw new ArgumentNullException(nameof(richTextBox));
        }

        // Trim buffer if exceeding max size
        TrimBuffer(richTextBox);

        // Append timestamp
        string timestamp = $"[{logEntry.Timestamp:HH:mm:ss.fff}] ";
        AppendText(richTextBox, timestamp, TimestampColor);

        // Append direction
        string direction = $"[{logEntry.Direction}] ";
        Color directionColor = logEntry.Direction == LogDirection.TX ? TxColor : RxColor;
        AppendText(richTextBox, direction, directionColor);

        // Append data
        AppendText(richTextBox, logEntry.Data + Environment.NewLine, directionColor);

        // Auto-scroll to bottom
        AutoScroll(richTextBox);
    }

    /// <summary>
    /// Clears all log entries from the RichTextBox.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to clear.</param>
    public static void ClearLog(RichTextBox richTextBox)
    {
        if (richTextBox == null)
        {
            throw new ArgumentNullException(nameof(richTextBox));
        }

        richTextBox.Clear();
    }

    /// <summary>
    /// Copies all log text from the RichTextBox to the clipboard.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to copy from.</param>
    public static void CopyLog(RichTextBox richTextBox)
    {
        if (richTextBox == null)
        {
            throw new ArgumentNullException(nameof(richTextBox));
        }

        if (!string.IsNullOrEmpty(richTextBox.Text))
        {
            Clipboard.SetText(richTextBox.Text);
        }
    }

    /// <summary>
    /// Appends colored text to a RichTextBox.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to append to.</param>
    /// <param name="text">The text to append.</param>
    /// <param name="color">The color of the text.</param>
    private static void AppendText(RichTextBox richTextBox, string text, Color color)
    {
        richTextBox.SelectionStart = richTextBox.TextLength;
        richTextBox.SelectionLength = 0;
        richTextBox.SelectionColor = color;
        richTextBox.AppendText(text);
        richTextBox.SelectionColor = richTextBox.ForeColor;
    }

    /// <summary>
    /// Scrolls the RichTextBox to the bottom.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to scroll.</param>
    private static void AutoScroll(RichTextBox richTextBox)
    {
        richTextBox.SelectionStart = richTextBox.TextLength;
        richTextBox.ScrollToCaret();
    }

    /// <summary>
    /// Trims the RichTextBox buffer to keep it under MaxBufferSize.
    /// Removes oldest lines until the buffer is under the limit.
    /// </summary>
    /// <param name="richTextBox">The RichTextBox to trim.</param>
    private static void TrimBuffer(RichTextBox richTextBox)
    {
        if (richTextBox.TextLength < MaxBufferSize)
        {
            return;
        }

        // Find the end of the first line to remove
        string text = richTextBox.Text;
        int newLineIndex = text.IndexOf('\n');

        if (newLineIndex < 0)
        {
            richTextBox.Clear();
            return;
        }

        // Remove lines until under the threshold (target: 80% of max)
        int targetSize = (int)(MaxBufferSize * 0.8);

        while (richTextBox.TextLength > targetSize)
        {
            int idx = richTextBox.Text.IndexOf('\n');
            if (idx < 0)
            {
                richTextBox.Clear();
                break;
            }

            richTextBox.Select(0, idx + 1);
            richTextBox.SelectedText = string.Empty;
        }
    }
}