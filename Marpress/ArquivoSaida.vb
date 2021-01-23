Imports System.IO

Namespace Funcoes

    Public Class ArquivoSaida

        Public Shared Sub escrever(ByVal diretorio As String, ByVal nomeArquivo As String, ByVal linha As String)
            Try
                Dim arquivo As StreamWriter
                If Not Directory.Exists(diretorio) Then
                    Directory.CreateDirectory(diretorio)
                End If
                If File.Exists(diretorio & nomeArquivo) Then
                    arquivo = New StreamWriter(diretorio & nomeArquivo, True)
                Else
                    arquivo = New StreamWriter(diretorio & nomeArquivo)
                End If
                arquivo.WriteLine(linha)
                arquivo.Flush()
                arquivo.Close()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

    End Class

End Namespace