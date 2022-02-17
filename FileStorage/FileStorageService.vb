Imports System.IO
Public Interface IFileStorageService
    Sub Upload(imageInByte As Byte(), id As Int32)
    Function Download(id As Int32) As byte()
End Interface

Public Class FileStorageService 
    Implements IFileStorageService
    Private ReadOnly _path = "\Files\Images\"
    
    Public Sub Upload(imageInByte As Byte(), id As Integer) Implements IFileStorageService.Upload
        Using stream As New FileStream(_path & id & ".jpg", FileMode.OpenOrCreate)
            stream.Write(imageInByte, 0, imageInByte.Length)
        End Using
    End Sub

    Public Function Download(id As Integer) As Byte() Implements IFileStorageService.Download
        Dim array
        Using stream As FileStream = File.OpenRead(_path & id & ".jpg")
            array = New Byte(stream.Length) {}
            stream.Read(array, 0, array.Length)
        End Using
        Return array
    End Function
End Class