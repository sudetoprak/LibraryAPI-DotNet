Bu proje; React arayüzü ile kurumsal .NET Web API altyapısını birleştiren, kitapların stok takibinden emanet işlemlerine kadar tüm sürecini N-Tier (Katmanlı) mimari ve Soft Delete gibi profesyonel standartlarla yöneten bir kütüphane otomasyon sistemidir.

1. **App.js**: Sistemin ana yönetim merkezidir. Veritabanından bilgileri getiren, güncelleyen ve diğer dosyalara API ile haberleşmek (kitap çekme, kiralama, silme komutlarını yönetmek) ve sayfadaki sekmeler arası geçişi sağlamak.

2. **BookList.js**: Kitapların ismini, yazarını ve stok durumunu gösterir. Kitapları listeler. Üzerindeki "Kirala" ve "Sil" butonları aracılığıyla kullanıcıdan gelen isteği App.js'e ileterek işlemi başlatmak.

3. **BookForm.js**: Kütüphaneye yeni bir kitap geldiğinde doldurulan kayıt formudur. Kitap adı, yazar ve stok gibi bilgileri kullanıcıdan alır. "Kaydet" butonuna basıldığında bu bilgileri paketleyip sisteme eklenmesi için merkeze göndermek.

4. **RentalList.js**: Kiralama geçmişini ve iade edilmemiş kitapları gösterir. Aktif kiralamaları listeler. Üzerindeki "İade Et" butonu ile kitabın kütüphaneye geri getirilme işlemini tetiklemek.

library-ui (Ana Klasör)
 ├── 📁 node_modules       
 ├── 📁 public             
 └── 📁 src 
      ├── 📄 App.js             Tüm veri trafiği burada döner
      ├── 📄 index.js           React'i HTML'e bağlayan kapı
      └── 📁 components         UZMANLAR: App.js'in yardımcısı dosyalar
           ├── 📄 BookList.js   --> (Kitapların bulunduğu yer)
           ├── 📄 BookForm.js   --> (Kitap kaydı )
           └── 📄 RentalList.js --> (kiralayanların listesi)

### Teknik Özellikler

1. **Bileşen Tabanlı Mimari**: Tüm arayüzü tek bir parça yerine BookList, BookForm, RentalList gibi bağımsız bileşenlere böldüm.
2. **State Management**: React'in useState hook'unu kullanarak uygulamanın "canlı" kalmasını sağladım.
3. **Asenkron API Entegrasyonu (Axios & Async/Await)**: Backend ile haberleşmek için Axios kütüphanesini kullandım.
4. **Dinamik Tab (Sekme) Sistemi**: Uygulamayı sekmeli bir yapıya (activeTab) oturttum.
5. **Koşullu Render (Conditional Rendering)**: Stok durumuna göre butonların davranışını değiştirdim.
6. **Responsive UI (Bootstrap 5)**: Bootstrap kütüphanesini kullanarak projenin her ekrana uyumlu olmasını sağladım.
7. **Error Handling (Hata Yönetimi)**: API'den dönen hataları yakalayarak alert mesajı gösteren mekanizma kurdum.




### Kullanılan Teknolojiler
- React JS
- Axios
- React Hooks
