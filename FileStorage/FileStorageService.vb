Imports System.IO
Imports Microsoft.AspNetCore.Http

Public Interface IFileStorageService
    Sub Upload(file As IFormFile, path As String)
    Function Download(path As String) As FileStream
End Interface

Public Class FileStorageService 
    Implements IFileStorageService
    
    Public Sub Upload(file As IFormFile, path As String) Implements IFileStorageService.Upload
        Using stream As New FileStream(path, FileMode.Create)
            file.CopyTo(stream)
        End Using
    End Sub

    Public Function Download(path As String) As FileStream Implements IFileStorageService.Download
        return new FileStream(path, FileMode.Open)
    End Function
End Class