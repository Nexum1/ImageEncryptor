# ImageEncryptor

### What is ImageEncryptor? ###

Embed and extract hidden files into and from PNG images

### How do I use ImageEncryptor? ###

* To encrypt files into an image:
```
 ImgEnc.Encrypt("BaseImage.png", new string[]{"fileToAddToBaseImage1.png"}, "SaveFileName.png");
 ```
* To Decrypt files from an already encrypted image:
```
var decrypted = ImgEnc.Decrypt("SaveFileName.png");
```
* To open one of the decrypted files:
```
File.WriteAllBytes("TempFileName.png", decrypted[0].Data);
System.Diagnostics.Process.Start("TempFileName.png");
```
* Or you can just look at the ImageEncryptorUI to see a fully working example!
### Contact ###

* If you have any ideas or issues, the issue tracker is your friend. Alternatively please email me (Corne Vermeulen) on Nexum1@live.com
