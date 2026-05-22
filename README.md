# Profesyonel Kütüphane Yönetim Sistemi

Bu proje, ASP.NET Core Web API ve React frontend ile geliştirilmiş bir **Kütüphane Yönetim Sistemi**dir. Kitap listeleme, kitap ekleme/güncelleme/silme, kiralama, iade takibi, geciken kiralamalar, kullanıcı girişi/kaydı ve rol yönetimi gibi temel kütüphane süreçlerini kapsar.

Proje, üniversite staj programı kapsamında bir web uygulamasının backend ve frontend altyapısı olarak hazırlanmıştır.

## Öne Çıkan Teknik Özellikler

* **Katmanlı Mimari:** Domain, Application, Infrastructure ve API katmanları ile sorumluluklar ayrılmıştır.
* **React Frontend:** Kitap işlemleri, kiralama/iade takibi, gecikenler ekranı ve admin kullanıcı yönetimi arayüz üzerinden kullanılabilir.
* **JWT Authentication:** Kullanıcı girişi token ile yapılır; korumalı endpointlere erişim için Bearer token kullanılır.
* **Rol Bazlı Yetkilendirme:** Admin, Staff ve Member rolleri desteklenir.
* **Admin Rol Yönetimi:** Admin kullanıcılar, sistemdeki kullanıcıların rollerini güncelleyebilir.
* **Global Soft Delete:** Kayıtlar veritabanından fiziksel olarak silinmez; `IsDeleted` alanı ile pasif hale getirilir.
* **EF Core Global Query Filters:** Silinmiş kayıtların listeleme sorgularına otomatik olarak dahil edilmemesi sağlanır.
* **Audit Fields:** Ortak entity kayıtlarında `CreatedAt` ve `UpdatedAt` alanları tutulur.
* **Merkezi Loglama Middleware:** Gelen HTTP istekleri ve dönen cevaplar özel middleware ile konsolda izlenir.
* **Global Exception Middleware:** API genelindeki hatalar merkezi middleware üzerinden yönetilir.
* **FluentValidation:** DTO doğrulamaları ayrı validator sınıfları ile yapılır.
* **Kitap Kapak Fotoğrafı:** Kitaplara görsel yüklenebilir; dosya yolu `PhotoUrl` alanında saklanır.
* **Borrower Snapshot:** Kiralama sırasında kullanıcı adı/e-posta bilgisi rental kaydına yazılır; kullanıcı silinse bile geçmiş kiralama bilgisinde isim kaybolmaz.
* **Code-First Migration:** Veritabanı şeması C# sınıfları ve EF Core migration dosyaları üzerinden yönetilir.
* **Swagger / OpenAPI:** Backend endpointleri Swagger arayüzü üzerinden test edilebilir.

## Kullanılan Teknolojiler

* **Backend:** .NET 8 / ASP.NET Core Web API
* **Frontend:** React, Axios, Bootstrap
* **Veritabanı:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Kimlik Doğrulama:** JWT Bearer Authentication
* **Validasyon:** FluentValidation
* **Dokümantasyon:** Swagger / OpenAPI
* **Loglama:** ILogger ve custom middleware

## Proje Dosya Yapısı

```text
LibraryManagement/
├── LibraryManagement.Domain/ (Öz Katman)
│   ├── BaseEntity.cs (Id, IsDeleted, CreatedAt, UpdatedAt)
│   └── Entities/
│       ├── Author.cs
│       ├── Book.cs
│       ├── Category.cs
│       ├── Rental.cs
│       ├── Role.cs
│       └── User.cs
├── LibraryManagement.Application/ (Uygulama Katmanı)
│   ├── DTOs/
│   │   ├── Requests/
│   │   │   ├── AuthorCreateDto.cs
│   │   │   ├── BookCreateDto.cs
│   │   │   ├── CategoryCreateDto.cs
│   │   │   ├── RentalCreateDto.cs
│   │   │   ├── UserCreateDto.cs
│   │   │   └── UserRoleUpdateDto.cs
│   │   └── Responses/
│   │       ├── AuthorDto.cs
│   │       ├── BookDto.cs
│   │       ├── CategoryDto.cs
│   │       ├── LoginDto.cs
│   │       ├── PagedResult.cs
│   │       ├── RegisterDto.cs
│   │       ├── ServiceResult.cs
│   │       └── UserDto.cs
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   ├── IAuthorService.cs
│   │   ├── IBookService.cs
│   │   ├── ICategoryService.cs
│   │   ├── IRentalService.cs
│   │   └── IUserService.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── AuthorService.cs
│   │   ├── BookService.cs
│   │   ├── CategoryService.cs
│   │   ├── RentalService.cs
│   │   └── UserService.cs
│   └── Validators/
│       ├── AuthorCreateDtoValidator.cs
│       ├── BookCreateDtoValidator.cs
│       ├── CategoryCreateDtoValidator.cs
│       ├── LoginDtoValidator.cs
│       ├── RegisterDtoValidator.cs
│       ├── RentalCreateDtoValidator.cs
│       └── UserCreateDtoValidator.cs
├── LibraryManagement.Infrastructure/ (Altyapı Katmanı)
│   ├── Context/
│   │   └── AppDbContext.cs
│   └── Migrations/
├── LibraryManagement.Api/ (Sunum Katmanı)
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── AuthorsController.cs
│   │   ├── BooksController.cs
│   │   ├── CategoriesController.cs
│   │   ├── RentalsController.cs
│   │   └── UsersController.cs
│   ├── Middleware/
│   │   ├── ExceptionMiddleware.cs
│   │   └── RequestResponseLoggingMiddleware.cs
│   ├── wwwroot/
│   │   └── uploads/books/ (Kitap kapak görselleri)
│   └── Program.cs
└── library-ui/ (React Ön Yüz)
    ├── public/
    └── src/
        ├── components/
        │   ├── BookForm.js
        │   ├── BookList.js
        │   ├── Login.js
        │   ├── RentalList.js
        │   └── UserAdminPanel.js
        ├── App.js
        ├── App.css
        ├── index.js
        └── index.css
```

## Mimari Yapı

Proje, sorumlulukların ayrılması prensibine uygun olarak dört ana katmandan oluşur:

1. **Domain Layer:** Entity sınıfları ve ortak `BaseEntity` yapısı burada bulunur.
2. **Application Layer:** Servisler, DTO'lar, servis arayüzleri ve validasyon kuralları burada yer alır.
3. **Infrastructure Layer:** `AppDbContext`, EF Core ayarları, global query filter ve migration dosyaları burada bulunur.
4. **API Layer:** Controller, middleware, authentication, authorization, CORS ve Swagger konfigürasyonları burada yönetilir.

## İlişkisel Veritabanı Tasarımı

Entity Framework Core kullanılarak kurulan veritabanı şemasında aşağıdaki ilişkiler yönetilmektedir:

* **User - Rental:** 1-N ilişki. Bir kullanıcı birden fazla kitap kiralayabilir.
* **Book - Rental:** 1-N ilişki. Bir kitap farklı zamanlarda birçok kez kiralanabilir.
* **Category - Book:** 1-N ilişki. Bir kategori birden fazla kitaba sahip olabilir.
* **Role - User:** Kullanıcı yetkileri rol bilgisi üzerinden yönetilir.
* **Data Integrity:** Foreign key kısıtlamaları ve Fluent API konfigürasyonları ile veri bütünlüğü korunur.

## Temel API Uç Noktaları

* **Auth:** Kullanıcı kaydı ve giriş işlemleri.
* **Books:** Kitap listeleme, arama, ekleme, güncelleme, kapak fotoğrafı yükleme ve soft delete.
* **Rentals:** Kitap kiralama, iade etme, kiralama geçmişi ve geciken kiralamalar.
* **Users:** Kullanıcı listeleme ve rol güncelleme.
* **Authors:** Yazar ekleme, listeleme, güncelleme ve silme işlemleri.
* **Categories:** Kategori ekleme, listeleme, güncelleme ve silme işlemleri.

## Frontend Ekranları

* **Giriş / Kayıt:** Kullanıcı girişi ve yeni kullanıcı kaydı.
* **Kitaplar:** Kitap listeleme, arama, kiralama, admin için ekleme/güncelleme/silme.
* **İade & Takip:** Kiralama geçmişi ve iade işlemleri.
* **Gecikenler:** Geciken kiralamaları listeleme ve arama.
* **Kullanıcılar:** Admin için kullanıcı rol yönetimi.

## Çalıştırma

Backend:

```powershell
cd C:\Users\sude\source\repos\LibraryManagement.Api\LibraryManagement.Api
dotnet run
```

Backend varsayılan olarak şu adreste çalışır:

```text
https://localhost:64610
```

Swagger:

```text
https://localhost:64610/swagger
```

Frontend:

```powershell
cd C:\Users\sude\source\repos\LibraryManagement.Api\library-ui
npm start
```

Frontend varsayılan olarak şu adreste açılır:

```text
http://localhost:3000
```

## Migration Komutları

Migration ekleme:

```powershell
dotnet ef migrations add MigrationName --project LibraryManagement.Infrastructure --startup-project LibraryManagement.Api
```

Veritabanını güncelleme:

```powershell
dotnet ef database update --project LibraryManagement.Infrastructure --startup-project LibraryManagement.Api
```

Eğer `dotnet ef` PATH içinde çalışmazsa:

```powershell
C:\Users\sude\.dotnet\tools\dotnet-ef.exe migrations add MigrationName --project LibraryManagement.Infrastructure --startup-project LibraryManagement.Api
C:\Users\sude\.dotnet\tools\dotnet-ef.exe database update --project LibraryManagement.Infrastructure --startup-project LibraryManagement.Api
```
