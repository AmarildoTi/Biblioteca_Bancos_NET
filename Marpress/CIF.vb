Namespace Padrao

    Public Class CIF

        Private _tipoRegistro As String = ""
        Public Property TipoRegistro() As String
            Get
                Return _tipoRegistro
            End Get
            Set(ByVal value As String)
                _tipoRegistro = value
            End Set
        End Property

        Private _codigoCIF As String
        Public Property CodigoCIF() As String
            Get
                Return _codigoCIF
            End Get
            Set(ByVal value As String)
                _codigoCIF = value
            End Set
        End Property

        Private _codigoCEP As String
        Public Property CodigoCEP() As String
            Get
                Return _codigoCEP
            End Get
            Set(ByVal value As String)
                _codigoCEP = value
            End Set
        End Property

        Private _codigoTriagem As String
        Public Property CodigoTriagem() As String
            Get
                Return _codigoTriagem
            End Get
            Set(ByVal value As String)
                _codigoTriagem = value
            End Set
        End Property

        Private _categoriaCEP As String
        Public Property CategoriaCEP() As String
            Get
                Return _categoriaCEP
            End Get
            Set(ByVal value As String)
                _categoriaCEP = value
            End Set
        End Property

        Private _codigoPostagem As String
        Public Property CodigoPostagem() As String
            Get
                Return _codigoPostagem
            End Get
            Set(ByVal value As String)
                _codigoPostagem = value
            End Set
        End Property

        Private _codigoAdministrativo As String
        Public Property CodigoAdministrativo() As String
            Get
                Return _codigoAdministrativo
            End Get
            Set(ByVal value As String)
                _codigoAdministrativo = value
            End Set
        End Property

        Private _IDV As String
        Public Property IDV() As String
            Get
                Return _IDV
            End Get
            Set(ByVal value As String)
                _IDV = value
            End Set
        End Property

        Private _CNAE As String
        Public Property CNAE() As String
            Get
                Return _CNAE
            End Get
            Set(ByVal value As String)
                _CNAE = value
            End Set
        End Property

        Private _servicoAdicional As String
        Public Property ServicoAdicional() As String
            Get
                Return _servicoAdicional
            End Get
            Set(ByVal value As String)
                _servicoAdicional = value
            End Set
        End Property

    End Class

End Namespace
