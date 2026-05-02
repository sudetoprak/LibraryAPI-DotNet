# Profesyonel Kütüphane Yönetim Sistemi

Bu proje, ASP.NET Core Web API ve React frontend ile geliştirilen bir **Kütüphane Yönetim Sistemi**dir. Kitap listeleme, kiralama, iade takibi, geciken kiralamalar, kullanıcı rol yönetimi ve kayıt/giriş işlemlerini içerir.

## Öne Çıkan Özellikler

* **Katmanlı Mimari:** Domain, Application, Infrastructure ve API katmanları ile sorumluluklar ayrılmıştır.
* **React Frontend:** Kitap işlemleri, kiralama/iade takibi, gecikenler ekranı ve admin kullanıcı yönetimi arayüzden kullanılabilir.
* **JWT Authentication:** Kullanıcı girişi token ile yapılır; roller token üzerinden okunur.
* **Rol Bazlı Yetkilendirme:** Admin, Staff ve Member rolleri desteklenir.
* **Admin Rol Yönetimi:** Admin kullanıcılar arayüzden kullanıcı rollerini değiştirebilir.
* **Soft Delete:** Kayıtlar fiziksel olarak silinmez; `IsDeleted` alanı ile pasif hale getirilir.
* **Audit Fields:** Tüm ortak entity kayıtlarında `CreatedAt` ve `UpdatedAt` alanları tutulur.
* **Geciken Kiralama Araması:** Geciken kiralamalar kitap adı veya kullanıcı adına göre filtrelenebilir.
* **Borrower Snapshot:** Kiralama sırasında kullanıcı adı/e-posta bilgisi rental kaydına yazılır; kullanıcı silinse bile geçmiş kiralama ekranında isim kaybolmaz.
* **Swagger / OpenAPI:** Backend endpointleri Swagger üzerinden test edilebilir.

## Kullanılan Teknolojiler

* **Backend:** .NET 8 / ASP.NET Core Web API
* **Frontend:** React, Axios, Bootstrap
* **Veritabanı:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Kimlik Doğrulama:** JWT Bearer Authentication
* **Dokümantasyon:** Swagger / OpenAPI

## Proje Yapısı

```text
LibraryManagement/
├── LibraryManagement.Domain/ (Öz Katman)
│   ├── BaseEntity.cs (Ortak özellikler: Id, IsDeleted, CreatedAt, UpdatedAt)
│   ├── LibraryManagement.Domain.csproj
│   └── Entities/
│       ├── Author.cs
│       ├── Book.cs
│       ├── BookAuthor.cs
│       ├── Category.cs
│       ├── User.cs
│       ├── Role.cs
│       └── Rental.cs
├── LibraryManagement.Application/ (Uygulama Katmanı)
│   ├── LibraryManagement.Application.csproj
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
│   │   ├── IAuthorService.cs
│   │   ├── IAuthService.cs
│   │   ├── IBookService.cs
│   │   ├── ICategoryService.cs
│   │   ├── IRentalService.cs
│   │   └── IUserService.cs
│   ├── Services/
│   │   ├── AuthorService.cs
│   │   ├── AuthService.cs
│   │   ├── BookService.cs
│   │   ├── CategoryService.cs
│   │   ├── RentalService.cs (Kiralama İş Mantığı)
│   │   ├── UserService.cs
│   └── Validators/
│       ├── AuthorCreateDtoValidator.cs
│       ├── BookCreateDtoValidator.cs
│       ├── CategoryCreateDtoValidator.cs
│       ├── LoginDtoValidator.cs
│       ├── RegisterDtoValidator.cs
│       ├── RentalCreateDtoValidator.cs
│       └── UserCreateDtoValidator.cs
├── LibraryManagement.Infrastructure/ (Altyapı Katmanı)
│   ├── LibraryManagement.Infrastructure.csproj
│   ├── Context/
│   │   └── AppDbContext.cs (EF Core Bağlantısı, Global Filtreler, Audit Fields)
│   └── Migrations/ (Veritabanı Geçmişi)
│       ├── InitialCreate
│       ├── GlobalSoftDeleteFix
│       ├── AddRentalReturnFields
│       ├── SeedRoles
│       ├── AddAuditFields
│       └── AddRentalBorrowerSnapshot
├── LibraryManagement.Api/ (Sunum Katmanı)
│   ├── LibraryManagement.Api.csproj
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── LibraryManagement.Api.http
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── AuthorsController.cs
│   │   ├── BooksController.cs
│   │   ├── CategoriesController.cs
│   │   ├── UsersController.cs
│   │   └── RentalsController.cs
│   ├── Middleware/
│   │   ├── ExceptionMiddleware.cs
│   │   └── RequestResponseLoggingMiddleware.cs (Anlık Trafik Loglama)
│   ├── Properties/
│   │   └── launchSettings.json
│   └── Program.cs (Uygulama Konfigürasyonu)
└── library-ui/ (React Ön Yüz)
    ├── package.json
    ├── public/
    │   ├── index.html
    │   └── manifest.json
    ├── src/
    │   ├── components/
    │   │   ├── BookForm.js
    │   │   ├── BookList.js
    │   │   ├── Login.js
    │   │   ├── RentalList.js
    │   │   └── UserAdminPanel.js
    │   ├── App.js
    │   ├── App.css
    │   ├── index.js
    │   └── index.css
    └── package-lock.json
```

## Backend Katmanları

1. **Domain Layer:** Entity sınıfları ve ortak `BaseEntity` yapısı burada bulunur.
2. **Application Layer:** Servisler, DTO'lar, arayüzler ve validasyonlar burada yer alır.
3. **Infrastructure Layer:** `AppDbContext`, EF Core ayarları, global query filter ve migration dosyaları burada bulunur.
4. **API Layer:** Controller, middleware, authentication ve Swagger konfigürasyonları burada yönetilir.

## Temel API Uç Noktaları

* **Auth:** Kayıt olma ve giriş işlemleri.
* **Books:** Kitap listeleme, ekleme, güncelleme ve soft delete.
* **Rentals:** Kitap kiralama, iade etme, kiralama geçmişi ve geciken kiralamalar.
* **Users:** Kullanıcı listeleme ve rol güncelleme.
* **Authors / Categories:** Yazar ve kategori işlemleri.

## Frontend Ekranları

* **Giriş / Kayıt:** Kullanıcı girişi ve yeni kullanıcı kaydı.
* **Kitaplar:** Kitap listeleme, kiralama, admin için ekleme/düzenleme/silme.
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
