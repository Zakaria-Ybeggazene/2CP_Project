﻿Public Class LoginWindow
    Private Sub Image1_ImageFailed(ByVal sender As System.Object, ByVal e As System.Windows.ExceptionRoutedEventArgs)

    End Sub

    Private Sub Image1_ImageFailed_1(ByVal sender As System.Object, ByVal e As System.Windows.ExceptionRoutedEventArgs)

    End Sub
    Private Sub userPassword_PasswordChanged(ByVal sender As System.Object, ByVal e As RoutedEventArgs)
        If userPassword.Password.Length = 0 Then
            userPasswordHint.Visibility = Windows.Visibility.Visible
        Else
            userPasswordHint.Visibility = Windows.Visibility.Hidden
        End If
        If wrongPasswordLabel.Visibility = Windows.Visibility.Visible Then
            wrongPasswordLabel.Visibility = Windows.Visibility.Hidden
        End If
    End Sub

    Private Sub Image1_ImageFailed_2(ByVal sender As System.Object, ByVal e As System.Windows.ExceptionRoutedEventArgs) Handles Image1.ImageFailed

    End Sub



    Private Sub tb_KeyDown(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles userPassword.PreviewKeyDown

        If (e.Key = Key.Enter) Then
            Try
                Me.ForceCursor = True
                Mouse.OverrideCursor = Cursors.Wait
                Repository.initialiser(userPassword.Password)
                Dim mainWindow As New MainWindow
                Me.Close()
                mainWindow.Show()
            Catch ex As Exception
                userPassword.Password = ""
                wrongPasswordLabel.Visibility = Windows.Visibility.Visible
            Finally
                Mouse.OverrideCursor = Nothing
            End Try
        End If
    End Sub

    Private Sub Login_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Login.Click
        Try
            Me.ForceCursor = True
            Mouse.OverrideCursor = Cursors.Wait
            Repository.initialiser(userPassword.Password)
            Dim mainWindow As New MainWindow
            Me.Close()
            mainWindow.Show()
        Catch ex As Exception
            userPassword.Password = ""
            wrongPasswordLabel.Visibility = Windows.Visibility.Visible
        Finally
            Mouse.OverrideCursor = Nothing
        End Try
    End Sub
End Class
