Imports System.IO

Namespace FAC

    Public Class MidiaCIF

        Public Shared Sub header(ByVal diretorio As String, ByVal nomeArquivo As String, ByVal codigoDR As Integer, ByVal codigoAdm As Integer, ByVal numeroCartao As Long, ByVal numeroLote As Integer, ByVal codigoUnidade As Integer, ByVal cepOrigem As Integer, ByVal numeroContrato As Long)
            Try
                Dim arquivo As StreamWriter
                If Not Directory.Exists(diretorio) Then
                    Directory.CreateDirectory(diretorio)
                End If
                arquivo = New StreamWriter(diretorio & nomeArquivo)
                arquivo.WriteLine("1" & codigoDR.ToString.Trim.PadLeft(2, "0") & _
                                  codigoAdm.ToString.Trim.PadLeft(8, "0") & _
                                  numeroCartao.ToString.Trim.PadLeft(12, "0") & _
                                  numeroLote.ToString.Trim.PadLeft(5, "0") & _
                                  codigoUnidade.ToString.Trim.PadLeft(8, "0") & _
                                  cepOrigem.ToString.Trim.PadLeft(8, "0") & _
                                  numeroContrato.ToString.Trim.PadLeft(10, "0"))
                arquivo.Flush()
                arquivo.Close()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Public Shared Sub detalhe(ByVal diretorio As String, ByVal nomeArquivo As String, ByVal idObjeto As String, ByVal pesoObjeto As Double, ByVal cepObjeto As Integer, ByVal codigoCategoria As String)
            Try
                Dim arquivo As StreamWriter
                If Not Directory.Exists(diretorio) Then
                    Directory.CreateDirectory(diretorio)
                End If
                If File.Exists(diretorio & nomeArquivo) Then
                    arquivo = New StreamWriter(diretorio & nomeArquivo, True)
                End If
                arquivo.WriteLine("2" & idObjeto.ToString.Trim.PadLeft(11, "0") & _
                                  String.Format("{0:0.00}", pesoObjeto).Trim.Replace(".", "").Replace(",", "").PadLeft(6, "0") & _
                                  cepObjeto.ToString.Trim.PadLeft(8, "0") & _
                                  codigoCategoria)
                arquivo.Flush()
                arquivo.Close()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Public Shared Sub trailer(ByVal diretorio As String, ByVal nomeArquivo As String, ByVal quantidade As Integer, ByVal peso As Double)
            Try
                Dim arquivo As StreamWriter
                If Not Directory.Exists(diretorio) Then
                    Directory.CreateDirectory(diretorio)
                End If
                arquivo = New StreamWriter(diretorio & nomeArquivo, True)
                arquivo.WriteLine("4" & quantidade.ToString.Trim.PadLeft(7, "0") & _
                                  String.Format("{0:0.00}", peso).Trim.Replace(".", "").Replace(",", "").PadLeft(10, "0"))
                arquivo.Flush()
                arquivo.Close()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

    End Class

End Namespace
