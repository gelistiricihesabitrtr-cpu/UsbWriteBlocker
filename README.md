# USB Write Blocker

Adli bilişim incelemeleri için Windows üzerinde yazılımsal USB write blocker. Etkinleştirildiğinde sisteme bağlı (takılı veya yeni takılacak) tüm USB kitle depolama aygıtlarını salt-okunur hale getirir; böylece incelenen delil diskine yanlışlıkla yazma, silme veya zaman damgası değişimi yapılmasını önler.

## Nasıl çalışır

Windows'un `HKLM\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies\WriteProtect` DWORD değerini kullanır. Bu değer `1` olduğunda işletim sistemi tüm USB depolama aygıtlarına yazmayı reddeder. Program bu registry değerini yönetmek için bir arayüz sunar.

**Not:** Bu, işletim sistemi seviyesinde yazılımsal bir korumadır — sertifikalı donanım write blocker'ların yerini almaz. Adli kabul edilebilirlik gerektiren resmi delil işlemlerinde donanım write blocker kullanımı değerlendirilmelidir.

## Kullanım

1. Programı **yönetici olarak** çalıştırın (UAC istemi otomatik çıkar; `HKLM` altında değişiklik yapabilmek için yönetici hakkı zorunludur).
2. İncelenecek USB aygıtı takmadan önce **"Yazma Korumasını Etkinleştir"** butonuna basın.
3. Aygıt zaten takılıysa, etkinleştirdikten sonra çıkarıp yeniden takın — aksi halde önceden bağlanmış (mount edilmiş) sürücü salt-okunur duruma geçmeyebilir.
4. Durum, hem pencere içindeki gösterge hem de sistem tepsisindeki (saat yanı) ikonla izlenebilir: **yeşil = korumasız**, **kırmızı = write blocker aktif**.
5. İnceleme bitince **"Yazma Korumasını Devre Dışı Bırak"** ile normal USB kullanımına dönün.

Koruma durumu Windows kayıt defterinde saklanır ve **kalıcıdır** — program kapatılsa veya bilgisayar yeniden başlatılsa bile ayar korunur; kapatmak için programı tekrar açıp devre dışı bırakmak gerekir.

## Derleme

.NET 8 SDK gerekir.

```
dotnet build
```

Tek dosya, bağımsız çalışan exe üretmek için:

```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

Çıktı: `bin/Release/net8.0-windows/win-x64/publish/UsbWriteBlocker.exe`

## Sorumluluk Reddi

Bu yazılım olduğu gibi sunulmuştur, herhangi bir garanti verilmez. Kritik/resmi adli inceleme süreçlerinde kullanmadan önce kendi ortamınızda doğrulayın ve gerektiğinde sertifikalı donanım çözümleriyle destekleyin.
