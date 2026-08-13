# Smart Tour Guide — Backend Progress Tracker
> ASP.NET Core 8 | Clean Architecture | SQL Server | SignalR | Paymob | JWT + Google OAuth

---

## 🚨 تعليمات للشات الجديد
> لما تفتح شات جديد مع كلود، ابدأ بالكلام ده:

**"احنا بنكمل مشروع TourGuide Backend، هنمسك [اسم الـ Feature] ونخلصها كاملة. الـ Feature دي محتاجة:**
1. DTOs في `TourGuide.Application/DTOs/[Feature]/`
2. Interface في `TourGuide.Application/Interfaces/`
3. Service في `TourGuide.Application/Services/`
4. Controller في `TourGuide.API/Controllers/`
5. تسجيل الـ DI في `ServiceCollectionExtensions.cs`
6. التأكد إن كل حاجة شغالة في Swagger

**مش هنسيب الـ Feature دي غير لما تظهر في Swagger وتشتغل."**

---

## 🗂️ Project Structure (ما اتعمل لحد دلوقتي)

```
TourGuide.sln
├── TourGuide.Domain
│   ├── Entities
│   │   ├── ApplicationUser.cs        ✅
│   │   ├── RefreshToken.cs           ✅
│   │   ├── GuideProfile.cs           ✅
│   │   ├── City.cs                   ✅
│   │   ├── GuideCity.cs              ✅
│   │   ├── Landmark.cs               ✅
│   │   ├── LandmarkImage.cs          ✅
│   │   ├── Package.cs                ✅
│   │   ├── PackageImage.cs           ✅
│   │   ├── PackageLandmark.cs        ✅
│   │   ├── Booking.cs                ✅
│   │   ├── Payment.cs                ✅
│   │   ├── Message.cs                ✅
│   │   ├── Review.cs                 ✅
│   │   └── Notification.cs           ✅
│   ├── Enums
│   │   ├── UserRole.cs               ✅
│   │   ├── BookingStatus.cs          ✅
│   │   ├── PaymentStatus.cs          ✅
│   │   ├── LandmarkCategory.cs       ✅
│   │   └── NotificationType.cs       ✅
│   ├── Interfaces
│   │   ├── IRepository.cs            ✅
│   │   ├── IUnitOfWork.cs            ✅
│   │   └── IChatRepository.cs        ✅
│   └── Exceptions
│       ├── NotFoundException.cs      ✅
│       ├── UnauthorizedException.cs  ✅
│       ├── BusinessRuleException.cs  ✅
│       └── ConflictException.cs      ✅
│
├── TourGuide.Application
│   ├── DTOs
│   │   ├── Auth
│   │   │   ├── RegisterRequest.cs       ✅
│   │   │   ├── LoginRequest.cs          ✅
│   │   │   ├── LoginResponse.cs         ✅
│   │   │   ├── GoogleAuthRequest.cs     ✅
│   │   │   ├── ForgetPasswordRequest.cs ✅
│   │   │   ├── ResetPasswordRequest.cs  ✅
│   │   │   ├── RefreshTokenRequest.cs   ✅
│   │   │   └── ChangePasswordRequest.cs ✅
│   │   ├── User
│   │   │   ├── UserDto.cs               ✅
│   │   │   ├── UpdateProfileRequest.cs  ✅
│   │   │   ├── AvatarResponse.cs        ✅
│   │   │   └── PaginatedUsersRequest.cs ✅
│   │   ├── City
│   │   │   ├── CityDto.cs               ✅
│   │   │   ├── CreateCityRequest.cs     ✅
│   │   │   └── UpdateCityRequest.cs     ✅
│   │   ├── Landmark
│   │   │   ├── LandmarkDto.cs           ✅
│   │   │   ├── CreateLandmarkRequest.cs ✅
│   │   │   ├── UpdateLandmarkRequest.cs ✅
│   │   │   └── LandmarkFilterParams.cs  ✅
│   │   ├── Guide
│   │   │   ├── GuideProfileDto.cs       ✅
│   │   │   ├── UpdateGuideRequest.cs    ✅
│   │   │   └── GuideListDto.cs          ✅
│   │   ├── Package
│   │   │   ├── PackageDto.cs               ✅
│   │   │   ├── CreatePackageRequest.cs     ✅
│   │   │   ├── UpdatePackageRequest.cs     ✅
│   │   │   ├── AddLandmarkToPackage.cs     ✅
│   │   │   └── PackageFilterParams.cs      ✅
│   │   ├── CustomTrip
│   │   │   ├── CalculatePriceRequest.cs    ✅
│   │   │   ├── CalculatePriceResponse.cs   ✅
│   │   │   ├── AvailableGuidesRequest.cs   ✅
│   │   │   └── CreateCustomTripRequest.cs  ✅
│   │   ├── Booking
│   │   │   ├── BookingDto.cs               ✅
│   │   │   ├── CreateBookingRequest.cs     ✅
│   │   │   ├── RejectBookingRequest.cs     ✅
│   │   │   └── BookingFilterParams.cs      ✅
│   │   ├── Payment
│   │   │   ├── InitiatePaymentRequest.cs   ✅
│   │   │   ├── InitiatePaymentResponse.cs  ✅
│   │   │   ├── PaymentStatusDto.cs         ✅
│   │   │   └── PaymobWebhookDto.cs         ✅
│   │   ├── Chat
│   │   │   ├── MessageDto.cs               ✅
│   │   │   ├── ConversationDto.cs          ✅
│   │   │   └── SendMessageRequest.cs       ✅
│   │   ├── Reviews
│   │   │   ├── CreateReviewRequest.cs      ✅
│   │   │   ├── UpdateReviewRequest.cs      ✅
│   │   │   └── ReviewDto.cs                ✅
│   │   ├── Notifications
│   │   │   └── NotificationDto.cs          ✅
│   │   └── Admin
│   │       ├── DashboardSummaryDto.cs      ✅
│   │       ├── BookingsReportDto.cs        ✅
│   │       ├── RevenueReportDto.cs         ✅
│   │       ├── TopCityDto.cs               ✅
│   │       ├── TopLandmarkDto.cs           ✅
│   │       ├── GuidePerformanceDto.cs      ✅
│   │       └── UserGrowthDto.cs            ✅
│   ├── Interfaces
│   │   ├── IAuthService.cs           ✅
│   │   ├── IJwtService.cs            ✅
│   │   ├── IEmailService.cs          ✅
│   │   ├── ICloudinaryService.cs     ✅
│   │   ├── IUserService.cs           ✅
│   │   ├── ICityService.cs           ✅
│   │   ├── ILandmarkService.cs       ✅
│   │   ├── IGuideService.cs          ✅
│   │   ├── IPackageService.cs        ✅
│   │   ├── ICustomTripService.cs     ✅
│   │   ├── IBookingService.cs        ✅
│   │   ├── IPaymentService.cs        ✅
│   │   ├── IPaymobService.cs         ✅
│   │   ├── IChatService.cs           ✅
│   │   ├── IReviewService.cs         ✅
│   │   ├── INotificationService.cs   ✅
│   │   └── IAdminService.cs          ✅
│   ├── Services
│   │   ├── AuthService.cs            ✅
│   │   ├── UserService.cs            ✅
│   │   ├── CityService.cs            ✅
│   │   ├── LandmarkService.cs        ✅
│   │   ├── GuideService.cs           ✅
│   │   ├── PackageService.cs         ✅
│   │   ├── CustomTripService.cs      ✅
│   │   ├── BookingService.cs         ✅
│   │   ├── PaymentService.cs         ✅
│   │   ├── ChatService.cs            ✅
│   │   ├── ReviewService.cs          ✅
│   │   ├── NotificationService.cs    ✅
│   │   └── AdminService.cs           ✅
│   ├── Validators                    ⬜
│   └── Mapping                       ⬜
│
├── TourGuide.Infrastructure
│   ├── Data
│   │   ├── AppDbContext.cs           ✅
│   │   └── Configurations
│   │       ├── BookingConfiguration.cs         ✅
│   │       ├── GuideCityConfiguration.cs       ✅
│   │       ├── PackageLandmarkConfiguration.cs ✅
│   │       ├── LandmarkConfiguration.cs        ✅
│   │       ├── PackageConfiguration.cs         ✅
│   │       ├── ReviewConfiguration.cs          ✅
│   │       ├── MessageConfiguration.cs         ✅
│   │       ├── NotificationConfiguration.cs    ✅
│   │       └── PaymentConfiguration.cs         ✅
│   ├── Repositories
│   │   ├── GenericRepository.cs      ✅
│   │   ├── UnitOfWork.cs             ✅
│   │   └── ChatRepository.cs         ✅
│   ├── Services
│   │   ├── EmailSettings.cs          ✅
│   │   ├── EmailService.cs           ✅ (implements IEmailService)
│   │   ├── CloudinarySettings.cs     ✅
│   │   ├── CloudinaryService.cs      ✅ (implements ICloudinaryService)
│   │   ├── PaymobSettings.cs         ✅
│   │   └── PaymobService.cs          ✅
│   ├── Identity
│   │   ├── JwtSettings.cs            ✅
│   │   ├── JwtService.cs             ✅ (implements IJwtService)
│   │   └── IdentitySeeder.cs         ✅
│   └── Hubs
│       ├── ChatHub.cs                ✅
│       └── NotificationHub.cs        ✅
│
└── TourGuide.API
    ├── Controllers
    │   ├── AuthController.cs              ✅ (9 endpoints)
    │   ├── UsersController.cs             ✅ (3 endpoints)
    │   ├── CitiesController.cs            ✅ (6 endpoints)
    │   ├── LandmarksController.cs         ✅ (7 endpoints)
    │   ├── GuidesController.cs            ✅ (4 endpoints)
    │   ├── PackagesController.cs          ✅ (11 endpoints)
    │   ├── CustomTripsController.cs       ✅ (3 endpoints)
    │   ├── BookingsController.cs          ✅ (9 endpoints)
    │   ├── PaymentsController.cs          ✅ (3 endpoints)
    │   ├── ChatController.cs              ✅ (3 endpoints)
    │   ├── ReviewsController.cs           ✅ (4 endpoints)
    │   ├── NotificationsController.cs     ✅ (4 endpoints)
    │   └── AdminController.cs             ✅ (15 endpoints)
    ├── Extensions
    │   └── ServiceCollectionExtensions.cs ✅
    ├── Middlewares
    │   ├── GlobalExceptionHandler.cs      ✅
    │   └── RequestLoggingMiddleware.cs    ✅
    └── Program.cs                         ✅
        ├── JWT Authentication             ✅
        ├── CORS (AllowCredentials)        ✅
        ├── SignalR AddSignalR()           ✅
        ├── Swagger + JWT Bearer           ✅
        ├── MapHub<ChatHub>                ✅
        └── MapHub<NotificationHub>        ✅
```

---

## ✅ Progress Overview

| # | Module | Status | Notes |
|---|--------|--------|-------|
| 00 | Solution & Project Setup | ✅ Done | |
| 01 | Domain — Entities | ✅ Done | 15 entities |
| 02 | Domain — Enums | ✅ Done | 5 enums |
| 03 | Domain — Interfaces (IRepository, IUnitOfWork) | ✅ Done | |
| 04 | Domain — Exceptions | ✅ Done | 4 exceptions |
| 05 | Infrastructure — AppDbContext + Configurations | ✅ Done | 9 configurations |
| 06 | Infrastructure — Generic Repository + UnitOfWork | ✅ Done | |
| 07 | Infrastructure — Identity Setup | ✅ Done | |
| 08 | Infrastructure — JWT Generation | ✅ Done | implements IJwtService |
| 09 | Infrastructure — Email Service (MailKit) | ✅ Done | implements IEmailService ⏳ Gmail Step 29 |
| 10 | Infrastructure — Cloudinary Service | ✅ Done | implements ICloudinaryService ⏳ Keys Step 29 |
| 11 | Infrastructure — Paymob Service | ✅ Done | ⏳ Keys + ngrok Step 29 |
| 12 | Infrastructure — SignalR Hubs | ✅ Done | ChatHub + NotificationHub |
| 13 | Auth — DTOs + Interface + Service + Controller | ✅ Done | 9 endpoints في Swagger ✅ |
| 14 | User — DTOs + Interface + Service + Controller | ✅ Done | 3 endpoints في Swagger ✅ |
| 15 | Cities & Landmarks — DTOs + Service + Controller | ✅ Done | 6 + 7 endpoints في Swagger ✅ |
| 16 | Guide — DTOs + Service + Controller | ✅ Done | 4 endpoints في Swagger ✅ |
| 17 | Packages — DTOs + Service + Controller | ✅ Done | 11 endpoints في Swagger ✅ |
| 18 | Custom Trip — DTOs + Service + Controller | ✅ Done | 3 endpoints في Swagger ✅ |
| 19 | Bookings — DTOs + Service + Controller | ✅ Done | 9 endpoints في Swagger ✅ |
| 20 | Payment — DTOs + Service + Controller | ✅ Done | 3 endpoints في Swagger ✅ |
| 21 | Chat — DTOs + Service + Controller + Hub | ✅ Done | 3 endpoints في Swagger ✅ |
| 22 | Reviews — DTOs + Service + Controller | ✅ Done | 4 endpoints في Swagger ✅ |
| 23 | Notifications — DTOs + Service + Controller | ✅ Done | 4 endpoints في Swagger ✅ |
| 24 | Admin Dashboard — DTOs + Service + Controller | ✅ Done | 15 endpoints في Swagger ✅ |
| 25 | API — Global Exception Middleware | ✅ Done | GlobalExceptionHandler + RequestLoggingMiddleware |
| 26 | API — DI Registration + Program.cs | ✅ Done | SignalR Hub mapping + CORS + JWT ✅ |
| 27 | API — Swagger + JWT + CORS Config | ✅ Done | Swagger JWT Bearer + CORS في Program.cs ✅ |
| 28 | EF Core — Migrations + Seed Data | ✅ Done | InitialCreate + Update-Database ✅ |
| 29 | Testing & Verification | ⬜ Not Started | ⏳ Gmail + Cloudinary + Paymob + ngrok |

---

## 🔄 الـ Feature Checklist (لكل شات جديد)
> كل feature لازم تخلص كل الخطوات دي قبل ما نعدي للـ feature التانية:

- [ ] DTOs (Request + Response) في `Application/DTOs/[Feature]/`
- [ ] Interface في `Application/Interfaces/I[Feature]Service.cs`
- [ ] Service في `Application/Services/[Feature]Service.cs`
- [ ] Controller في `API/Controllers/[Feature]Controller.cs`
- [ ] تسجيل الـ DI في `ServiceCollectionExtensions.cs`
- [ ] Build ✅ بدون errors
- [ ] ظهور الـ endpoints في Swagger ✅
- [ ] تحديث ملف الـ Progress ✅

---

## ⏳ Pending External Config (هيتعمل في Step 29)

| Service | ما محتاجه | ملاحظة |
|---------|-----------|--------|
| Gmail SMTP | Username + App Password | EmailService جاهز |
| Cloudinary | CloudName + ApiKey + ApiSecret | CloudinaryService جاهز |
| Paymob | ApiKey + IntegrationId + IframeId + HmacSecret | PaymobService جاهز |
| Google OAuth | ClientId | JwtService جاهز |

---

## 📋 Detailed Steps Log

### 00 — Solution & Project Setup ✅
- [x] Create solution `TourGuide.sln`
- [x] Create 4 projects: Domain, Application, Infrastructure, API
- [x] Add project references: API → Infrastructure → Application → Domain
- [x] Install NuGet packages per project
- [x] Setup `appsettings.json` keys

---

### 01 — Domain — Entities ✅
- [x] `ApplicationUser`, `RefreshToken`, `GuideProfile`, `City`, `GuideCity`
- [x] `Landmark`, `LandmarkImage`, `Package`, `PackageImage`, `PackageLandmark`
- [x] `Booking`, `Payment`, `Message`, `Review`, `Notification`

---

### 02 — Domain — Enums ✅
- [x] `UserRole`, `BookingStatus`, `PaymentStatus`, `LandmarkCategory`, `NotificationType`

---

### 03 — Domain — Interfaces ✅
- [x] `IRepository<T>`, `IUnitOfWork`, `IChatRepository`

---

### 04 — Domain — Exceptions ✅
- [x] `NotFoundException`, `UnauthorizedException`, `BusinessRuleException`, `ConflictException`

---

### 05 — Infrastructure — AppDbContext + Configurations ✅
- [x] `AppDbContext.cs` — DbSets + ApplyConfigurationsFromAssembly
- [x] 9 Configurations: Booking, GuideCity, PackageLandmark, Landmark, Package, Review, Message, Notification, Payment

---

### 06 — Infrastructure — Repository + UnitOfWork ✅
- [x] `GenericRepository<T>`, `UnitOfWork`, `ChatRepository`

---

### 07 — Infrastructure — Identity Setup ✅
- [x] `IdentitySeeder` — Roles + Admin (admin@tourguide.com / Admin@123456)

---

### 08 — Infrastructure — JWT Generation ✅
- [x] `JwtSettings`, `JwtService : IJwtService`
- [x] GenerateAccessTokenAsync, GenerateRefreshToken

---

### 09 — Infrastructure — Email Service ✅
- [x] `EmailSettings`, `EmailService : IEmailService`
- [x] Templates: Confirmation, Reset, Rejection, Approval
- [ ] ⏳ Gmail credentials — Step 29

---

### 10 — Infrastructure — Cloudinary Service ✅
- [x] `CloudinarySettings`, `CloudinaryService : ICloudinaryService`
- [x] UploadImageAsync, DeleteImageAsync
- [ ] ⏳ Cloudinary keys — Step 29

---

### 11 — Infrastructure — Paymob Service ✅
- [x] `PaymobSettings`, `PaymobService`
- [x] GetAuthTokenAsync, CreateOrderAsync, GetPaymentKeyAsync, ValidateHmac
- [ ] ⏳ Paymob keys + ngrok — Step 29

---

### 12 — Infrastructure — SignalR Hubs ✅
- [x] `ChatHub` — SendMessage, MarkAsRead, Presence
- [x] `NotificationHub` — push via IHubContext
- [x] MapHub في Program.cs: /hubs/chat + /hubs/notifications

---

### 13 — Auth ✅
- [x] DTOs: RegisterRequest, LoginRequest, LoginResponse, GoogleAuthRequest
- [x] DTOs: ForgetPasswordRequest, ResetPasswordRequest, RefreshTokenRequest, ChangePasswordRequest
- [x] `IAuthService`, `IJwtService`, `IEmailService`, `ICloudinaryService` interfaces
- [x] `AuthService` implementation
- [x] `AuthController` — 9 endpoints
- [x] Build ✅ — Swagger ✅
- [ ] ⏳ تيست Register + Login + Confirm Email — Step 29

---

### 14 — User ✅
- [x] DTOs: UserDto, UpdateProfileRequest, AvatarResponse, PaginatedUsersRequest
- [x] `IUserService`, `UserService`
- [x] `UsersController` — 3 endpoints (me, update, avatar)
- [x] Build ✅ — Swagger ✅

---

### 15 — Cities & Landmarks ✅
- [x] DTOs: CityDto, CreateCityRequest, UpdateCityRequest, LandmarkDto, CreateLandmarkRequest, UpdateLandmarkRequest, LandmarkFilterParams
- [x] `ICityService`, `CityService`, `CitiesController` — 6 endpoints
- [x] `ILandmarkService`, `LandmarkService`, `LandmarksController` — 7 endpoints
- [x] Build ✅ — Swagger ✅

---

### 16 — Guide ✅
- [x] DTOs: GuideProfileDto, UpdateGuideRequest, GuideListDto
- [x] `IGuideService`, `GuideService`
- [x] `GuidesController` — 4 endpoints
- [x] Admin endpoints → AdminController
- [x] Build ✅ — Swagger ✅

---

### 17 — Packages ✅
- [x] DTOs: PackageDto, CreatePackageRequest, UpdatePackageRequest, AddLandmarkToPackage, PackageFilterParams
- [x] `IPackageService`, `PackageService`
- [x] `PackagesController` — 11 endpoints
- [x] Build ✅ — Swagger ✅

---

### 18 — Custom Trip ✅
- [x] DTOs: CalculatePriceRequest, CalculatePriceResponse, AvailableGuidesRequest, CreateCustomTripRequest
- [x] `ICustomTripService`, `CustomTripService`
- [x] `CustomTripsController` — 3 endpoints
- [x] Build ✅ — Swagger ✅

---

### 19 — Bookings ✅
- [x] DTOs: BookingDto, CreateBookingRequest, RejectBookingRequest, BookingFilterParams
- [x] `IBookingService`, `BookingService`
- [x] `BookingsController` — 9 endpoints
- [x] Build ✅ — Swagger ✅

---

### 20 — Payment ✅
- [x] DTOs: InitiatePaymentRequest, InitiatePaymentResponse, PaymentStatusDto, PaymobWebhookDto
- [x] `IPaymentService`, `IPaymobService`, `PaymentService`
- [x] `PaymentsController` — 3 endpoints
- [x] Build ✅ — Swagger ✅
- [ ] ⏳ تيست مع Paymob keys + ngrok — Step 29

---

### 21 — Chat ✅
- [x] DTOs: MessageDto, ConversationDto, SendMessageRequest
- [x] `IChatRepository`, `ChatRepository`, `IChatService`, `ChatService`
- [x] `ChatController` — 3 endpoints
- [x] Build ✅ — Swagger ✅

---

### 22 — Reviews ✅
- [x] DTOs: CreateReviewRequest, UpdateReviewRequest, ReviewDto
- [x] `IReviewService`, `ReviewService`
- [x] `ReviewsController` — 4 endpoints
- [x] Admin endpoints → AdminController
- [x] Build ✅ — Swagger ✅

---

### 23 — Notifications ✅
- [x] DTOs: NotificationDto
- [x] `INotificationService`, `NotificationService`
- [x] `NotificationsController` — 4 endpoints
- [x] Build ✅ — Swagger ✅

---

### 24 — Admin Dashboard ✅
- [x] DTOs: DashboardSummaryDto, BookingsReportDto, RevenueReportDto, TopCityDto, TopLandmarkDto, GuidePerformanceDto, UserGrowthDto
- [x] `IAdminService`, `AdminService`
- [x] `AdminController` — 15 endpoints
- [x] Build ✅ — Swagger ✅

---

### 25 — API — Global Exception Middleware ✅
- [x] `GlobalExceptionHandler` — catches Domain exceptions → standard ApiResponse
- [x] `RequestLoggingMiddleware` — logs method, path, status, duration
- [x] Registered في Program.cs

---

### 26 — API — DI Registration + Program.cs ✅
- [x] `ServiceCollectionExtensions` — كل الـ services مسجلين
- [x] JWT Authentication + Authorization
- [x] CORS — AllowCredentials للـ SignalR
- [x] SignalR — AddSignalR() + MapHub
- [x] Swagger + JWT Bearer button
- [x] Middleware pipeline كامل
- [x] IdentitySeeder في startup
- [x] MapHub<ChatHub>("/hubs/chat")
- [x] MapHub<NotificationHub>("/hubs/notifications")

---

### 27 — API — Swagger + JWT + CORS ✅
- [x] Swagger JWT Bearer button
- [x] CORS policy مع AllowCredentials

---

### 28 — EF Core Migrations ✅
- [x] `Add-Migration InitialCreate`
- [x] `Update-Database` — TourGuideDb اتعملت بنجاح
- [x] Seed Roles (Tourist, Guide, Admin) + Admin user

---

### 29 — Testing & Verification ⬜
- [ ] Swagger — test all endpoints
- [ ] SignalR — Chat + Notifications
- [ ] ⏳ Gmail App Password — حطه في appsettings + تيست confirmation email
- [ ] ⏳ Cloudinary keys — حطهم في appsettings + تيست upload
- [ ] ⏳ Paymob keys — حطهم في appsettings + تيست payment flow
- [ ] ⏳ ngrok — expose localhost للـ Paymob webhook
- [ ] ⏳ Google ClientId — حطه في appsettings + تيست Google OAuth

---

## 🔑 appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-09JL97C\\SQLEXPRESS;Database=TourGuideDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JWT": {
    "Key": "YourSuperSecretKeyHereMustBe32CharsMin!!",
    "Issuer": "TourGuideAPI",
    "Audience": "TourGuideClient",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  },
  "Google": { "ClientId": "" },
  "Cloudinary": { "CloudName": "", "ApiKey": "", "ApiSecret": "" },
  "Paymob": { "ApiKey": "", "IntegrationId": "", "IframeId": "", "HmacSecret": "" },
  "Email": { "Host": "smtp.gmail.com", "Port": 587, "Username": "", "Password": "", "From": "" }
}
```

---

## 📌 Legend
| Icon | Meaning |
|------|---------|
| ⬜ | Not Started |
| 🔄 | In Progress |
| ✅ | Done |
| ⚠️ | Has Issue |
| ⏳ | Needs External Config / Test in Step 29 |