Imports System.ComponentModel
Imports System.Reflection

Namespace FichaCompensacao

    Public Enum TipoCobranca
        <Description("Arrecadação")> ARRECADACAO
        <Description("Compensação")> COMPENSACAO
    End Enum

End Namespace

Namespace FAC

    ''' <summary>
    ''' Enumerador com as opções de contratos FAC
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public Enum Contrato
        <Description("Associação Comercial de São Paulo")> ACSP
        '<Description("Banco do Brasil")> BB
        '<Description("Bradesco")> BRADESCO
        <Description("Boa Vista Serviços")> BVS
        <Description("DmCard")> DMCARD
        <Description("DmCard - Catões")> DMCARD_CARTOES
        <Description("Hoepers")> HOEPERS
        <Description("Lopes Supermercados")> LOPES
        <Description("OMNI")> OMNI
        <Description("Oscar Calçados")> OSCAR
        <Description("Jô Calçados")> JOCALCADOS
        '<Description("Reaval")> REAVAL
        '<Description("Zanc")> ZANC
    End Enum

    Public Enum Processamento As Byte
        <Description("Produção")> PRODUCAO = True
        <Description("Teste")> TESTE = False
    End Enum

    Public Enum Tipo
        <Description("Simples")> SIMPLES
        <Description("Registrado")> REGISTRADO
        <Description("Registrado Com AR")> REGISTRADO_COM_AR
    End Enum

    Public Class Enumeradores

        ''' <summary>
        ''' Obtém a descrição de um determinado Enumerador.
        ''' </summary>
        ''' <param name="valor">Enumerador que terá a descrição obtida</param>
        ''' <returns>String com a descrição do Enumerador</returns>
        ''' <remarks></remarks>
        Public Shared Function ObterDescricao(ByVal valor As [Enum]) As String
            Dim info As FieldInfo = valor.GetType.GetField(valor.ToString)
            Dim atributos As DescriptionAttribute() = DirectCast(info.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())

            Return IIf(atributos.Length > 0, IIf(atributos(0).Description Is Nothing, "Nulo", atributos(0).Description), valor.ToString)
        End Function

        Public Shared Function Listar(ByVal tipo As Type) As IList
            Dim lista As New ArrayList
            If tipo IsNot Nothing Then
                Dim enumValores As Array = [Enum].GetValues(tipo)
                For Each valor As [Enum] In enumValores
                    lista.Add(New KeyValuePair(Of [Enum], String)(valor, ObterDescricao(valor)))
                Next
            End If
            Return lista
        End Function

    End Class

End Namespace
