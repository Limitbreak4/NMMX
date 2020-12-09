Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web.Security.AntiXss

Public Class cls_EncriptacionRSA

    Public Function encriptar128BitRijndael(
            ByVal textoEncriptar As String,
            ByVal claveEncriptacion As String) As String
        Dim bytValue() As Byte
        Dim bytKey() As Byte
        Dim bytEncoded() As Byte = New Byte() {}
        Dim bytIV() As Byte = {121, 241, 10, 1, 132,
                               74, 11, 39, 255, 91, 45,
                               78, 14, 211, 22, 62}
        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim objMemoryStream As New MemoryStream()
        Dim objCryptoStream As CryptoStream
        Dim objRijndaelManaged As RijndaelManaged
        textoEncriptar = AntiXssEncoder.HtmlEncode(textoEncriptar, False)
        claveEncriptacion = AntiXssEncoder.HtmlEncode(claveEncriptacion, False)
        'Quitar nulos en cadena de texto a encriptar
        textoEncriptar = quitarNullCadena(textoEncriptar)

        If textoEncriptar = "" Then
            Return ""
        End If

        bytValue = Encoding.ASCII.GetBytes(textoEncriptar.ToCharArray)

        intLength = Len(claveEncriptacion)


        'La clave de cifrado debe ser de 256 bits de longitud (32 bytes)
        'Si tiene más de 32 bytes se truncará
        'Si es menor de 32 bytes se rellenará con X
        If intLength >= 32 Then
            claveEncriptacion = Strings.Left(claveEncriptacion, 32)
        Else
            intLength = Len(claveEncriptacion)
            intRemaining = 32 - intLength
            claveEncriptacion = claveEncriptacion & Strings.StrDup(intRemaining, "X")
        End If

        bytKey = Encoding.ASCII.GetBytes(claveEncriptacion.ToCharArray)

        objRijndaelManaged = New RijndaelManaged()

        Try
            'Crear objeto Encryptor y escribir su valor 
            'después de que se convierta en array de bytes
            objCryptoStream = New CryptoStream(objMemoryStream, _
              objRijndaelManaged.CreateEncryptor(bytKey, bytIV), _
              CryptoStreamMode.Write)
            objCryptoStream.Write(bytValue, 0, bytValue.Length)

            objCryptoStream.FlushFinalBlock()

            bytEncoded = objMemoryStream.ToArray
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch ex As Exception
            MsgBox("Error al encriptar cadena de texto: " &
                   ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
        End Try

        'Devolver el valor del texto encriptado
        'convertido de array de bytes a texto en base64
        Return Convert.ToBase64String(bytEncoded)
    End Function



    Public Function desencriptar128BitRijndael(
            ByVal textoEncriptado As String,
            ByVal claveDesencriptacion As String) As String
        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132,
                               74, 11, 39, 255, 91,
                               45, 78, 14, 211, 22, 62}
        Dim objRijndaelManaged As New RijndaelManaged()
        Dim objMemoryStream As MemoryStream
        Dim objCryptoStream As CryptoStream
        Dim bytDecryptionKey() As Byte

        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim strReturnString As String = String.Empty
        textoEncriptado = AntiXssEncoder.HtmlEncode(textoEncriptado, False)
        claveDesencriptacion = AntiXssEncoder.HtmlEncode(claveDesencriptacion, False)

        If textoEncriptado = "" Then
            Return ""
        End If

        'Convertir el valor encriptado base64 a array de bytes
        bytDataToBeDecrypted = Convert.FromBase64String(textoEncriptado)

        'La clave de desencriptación debe ser de 256 bits de longitud (32 bytes)
        'Si tiene más de 32 bytes se truncará
        'Si es menor de 32 bytes se rellenará con A
        intLength = Len(claveDesencriptacion)

        If intLength >= 32 Then
            claveDesencriptacion = Strings.Left(claveDesencriptacion, 32)
        Else
            intLength = Len(claveDesencriptacion)
            intRemaining = 32 - intLength
            claveDesencriptacion = claveDesencriptacion & Strings.StrDup(intRemaining, "X")
        End If

        bytDecryptionKey = Encoding.ASCII.GetBytes(claveDesencriptacion.ToCharArray)

        ReDim bytTemp(bytDataToBeDecrypted.Length)

        objMemoryStream = New MemoryStream(bytDataToBeDecrypted)

        Try
            'Crear objeto Dencryptor y escribir su valor 
            'después de que se convierta en array de bytes
            objCryptoStream = New CryptoStream(objMemoryStream, _
               objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), _
               CryptoStreamMode.Read)

            objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

            'objCryptoStream.FlushFinalBlock()
            objMemoryStream.Close()
            objCryptoStream.Close()
        Catch ex As Exception
            MsgBox("Error al desencriptar cadena de texto: " &
                   ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
        End Try

        'Devolver la cadena de texto desencriptada
        'convertida de array de bytes a cadena de texto ASCII
        Return quitarNullCadena(Encoding.ASCII.GetString(bytTemp))
    End Function

    'Quita nulos de una cadena de texto
    Public Function quitarNullCadena(
            ByVal texto As String) As String
        Dim posicionNull As Integer
        Dim textoSinNull As String

        posicionNull = 1
        textoSinNull = texto

        Do While posicionNull > 0
            posicionNull = InStr(posicionNull, texto, vbNullChar)

            If posicionNull > 0 Then
                textoSinNull =
                    Left$(textoSinNull, posicionNull - 1) & _
                    Right$(textoSinNull,
                    Len(textoSinNull) - posicionNull)
            End If

            If posicionNull > textoSinNull.Length Then
                Exit Do
            End If
        Loop

        Return textoSinNull
    End Function
End Class
