Imports System.Windows.Forms

Module Program
    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    <STAThread>
    Sub Main()
        ' Enable visual styles and set compatible text rendering default
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Set the application's unhandled exception mode
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)

        ' Add global exception handlers
        AddHandler Application.ThreadException, AddressOf Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException

        Try
            ' Run the main form
            Application.Run(New frmMain())
        Catch ex As Exception
            MessageBox.Show($"A critical error occurred: {ex.Message}",
                          "Critical Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Application_ThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
        Try
            Dim errorMessage As String = $"An unexpected error occurred: {e.Exception.Message}{vbCrLf}{vbCrLf}" &
                                       $"Details: {e.Exception.ToString()}"

            MessageBox.Show(errorMessage,
                          "Application Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Catch
            ' If even the error handler fails, try to show a simple message
            MessageBox.Show("A critical error occurred and the application needs to close.",
                          "Critical Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CurrentDomain_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        Try
            Dim ex As Exception = TryCast(e.ExceptionObject, Exception)
            Dim errorMessage As String

            If ex IsNot Nothing Then
                errorMessage = $"An unhandled error occurred: {ex.Message}{vbCrLf}{vbCrLf}" &
                             $"Details: {ex.ToString()}"
            Else
                errorMessage = "An unknown unhandled error occurred."
            End If

            MessageBox.Show(errorMessage,
                          "Unhandled Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        Catch
            ' Last resort error message
            MessageBox.Show("A critical unhandled error occurred.",
                          "Critical Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub
End Module