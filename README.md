Vessel HydroStatic Calculater programı.
Girilen draft ve trim değerlerini bünyesinde bulunan datalar ışıgında hesaplayan bir programdır.

1.	Program açılırken tüm değerleri txt dosyasından okuyup ara belleğe [,] liste olarak otomatik yazıyor.
2.	Trim değerlerini txt dosyasından otomatik bulup property e yazıyor.
3.	Formda dropbox olmalı (Trim değerlerini içermeli) ve seçilen trim değerine göre ilgili tablosunu otomatik altındaki tabloya getiriyo.
4.	2 adet girdi “box” mevcut:
a.	Trim değeri: kullanıcı tarafından girilebilir 
b.	Draft değeri: kullanıcı tarafından girilebilir.
c.	Bu trim ve draft değerine göre diğer değişkenleri program otomatikman veriyor. Bunu yaparken lineer interpolasyon kullanılıyor.

Örnek olarak:
Kullanıcı a kutusuna -3,7 trim ve b kutusuna 1,02 metre draft değerleri girsin.
a.	Program -3,7 m trim değerine en yakın 2 tabloyu buluyor (-4,0 ve -3,5trim tabloları)
b.	Program 1,02 m draft değerine en yakın 2 satırları buluyor. (1,00 ve 1,03)
c.	Bunları kendi arasında interpole ediyor ve nihati sonuçları veriyor. (toplam 3 defa interpolasyon var. (X) satırını bulacak, daha sonra (Y) satırını bulacak. Daha sonra (X) ve (Y) satırlarının da interpolasyonunu yapacak.
