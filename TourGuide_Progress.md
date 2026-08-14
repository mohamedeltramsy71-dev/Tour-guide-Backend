# Smart Tour Guide — Backend Progress Tracker
> ASP.NET Core 8 | Clean Architecture | SQL Server | SignalR | Paymob | JWT + Google OAuth

---

## 🗂️ Project Structure

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
| 03 | Domain — Interfaces | ✅ Done | IRepository, IUnitOfWork, IChatRepository |
| 04 | Domain — Exceptions | ✅ Done | 4 exceptions |
| 05 | Infrastructure — AppDbContext + Configurations | ✅ Done | 9 configurations |
| 06 | Infrastructure — Generic Repository + UnitOfWork | ✅ Done | |
| 07 | Infrastructure — Identity Setup | ✅ Done | Roles + Admin seeder |
| 08 | Infrastructure — JWT Generation | ✅ Done | implements IJwtService |
| 09 | Infrastructure — Email Service | ✅ Done | Gmail SMTP ✅ |
| 10 | Infrastructure — Cloudinary Service | ✅ Done | Cloudinary keys ✅ |
| 11 | Infrastructure — Paymob Service | ✅ Done | Paymob Sandbox keys ✅ |
| 12 | Infrastructure — SignalR Hubs | ✅ Done | ChatHub + NotificationHub |
| 13 | Auth | ✅ Done | 9 endpoints — Register ✅ Login ✅ Email Confirm ✅ |
| 14 | User | ✅ Done | 3 endpoints — Avatar Upload ✅ |
| 15 | Cities & Landmarks | ✅ Done | 6 + 7 endpoints |
| 16 | Guide | ✅ Done | 4 endpoints |
| 17 | Packages | ✅ Done | 11 endpoints |
| 18 | Custom Trip | ✅ Done | 3 endpoints |
| 19 | Bookings | ✅ Done | 9 endpoints |
| 20 | Payment | ✅ Done | 3 endpoints — ⏳ ngrok webhook test |
| 21 | Chat | ✅ Done | 3 endpoints + SignalR Hub |
| 22 | Reviews | ✅ Done | 4 endpoints |
| 23 | Notifications | ✅ Done | 4 endpoints |
| 24 | Admin Dashboard | ✅ Done | 15 endpoints |
| 25 | API — Global Exception Middleware | ✅ Done | |
| 26 | API — DI Registration + Program.cs | ✅ Done | |
| 27 | API — Swagger + JWT + CORS | ✅ Done | |
| 28 | EF Core — Migrations + Seed Data | ✅ Done | TourGuideDb ✅ |
| 29 | Testing & Verification | 🔄 In Progress | Paymob webhook ⏳ |

---

## 🔑 External Services Config

| Service | Status | Notes |
|---------|--------|-------|
| Gmail SMTP | ✅ Done | mohamedeltramsy71@gmail.com |
| Cloudinary | ✅ Done | Cloud: dp1po0xxf |
| Google OAuth | ✅ Done | Client ID configured |
| Paymob Sandbox | ✅ Done | Integration: 5853399 — Iframe: 1069052 |

---

## 📋 Detailed Steps Log

### 01 — Domain — Entities ✅
- [x] `ApplicationUser` (extends IdentityUser) — FullName, Bio, Phone, AvatarUrl, IsDeleted, IsBanned
- [x] `RefreshToken` — Token, ExpiresAt, IsRevoked, UserId
- [x] `GuideProfile` — Bio, ExperienceYears, AverageRating, IsApproved, IsSuspended, LanguagesJson
- [x] `City` — NameAr, NameEn, Description, ImageUrl, IsDeleted
- [x] `GuideCity` — composite key (GuideProfileId, CityId)
- [x] `Landmark` — NameAr, NameEn, Category, EntryFee, CityId, IsDeleted
- [x] `LandmarkImage` — ImageUrl, PublicId, LandmarkId
- [x] `Package` — Title, Price, DurationDays, MaxPersons, CityId, GuideProfileId, IsDeleted
- [x] `PackageImage` — ImageUrl, PublicId, PackageId
- [x] `PackageLandmark` — composite key (PackageId, LandmarkId), DayNumber, Order
- [x] `Booking` — StartDate, NumberOfPersons, TotalPrice, Status, PaymentStatus, IsCustom
- [x] `Payment` — PaymobOrderId, PaymobTransactionId, Amount, Status, BookingId
- [x] `Message` — Content, IsRead, SenderId, BookingId
- [x] `Review` — Rating (1-5), Comment, TouristId, GuideProfileId, BookingId
- [x] `Notification` — Message, Type, IsRead, UserId, BookingId

---

### 02 — Domain — Enums ✅
- [x] `UserRole` — Tourist, Guide, Admin
- [x] `BookingStatus` — Pending, Confirmed, Rejected, Cancelled, Completed
- [x] `PaymentStatus` — Unpaid, Paid, Failed
- [x] `LandmarkCategory` — Historical, Entertainment, Nature, Religious, Beach, Museum
- [x] `NotificationType` — NewBooking, BookingAccepted, BookingRejected, PaymentConfirmed, NewMessage, GuideApproved, TripReminder

---

### 03 — Domain — Interfaces ✅
- [x] `IRepository<T>` — GetById, GetAll, Find, FindOne, Add, Update, Delete, Exists, Count
- [x] `IUnitOfWork` — Repository<T>(), SaveChangesAsync()
- [x] `IChatRepository` — custom chat queries

---

### 04 — Domain — Exceptions ✅
- [x] `NotFoundException` — 404
- [x] `UnauthorizedException` — 401
- [x] `BusinessRuleException` — 400
- [x] `ConflictException` — 409

---

### 05 — Infrastructure — AppDbContext + Configurations ✅
- [x] `AppDbContext` extends `IdentityDbContext<ApplicationUser>`
- [x] `ApplyConfigurationsFromAssembly` — كل الـ configurations تتحمل أوتوماتيك
- [x] Soft delete query filters: ApplicationUser, City, Landmark, Package
- [x] 9 Entity Configurations: Booking, GuideCity, PackageLandmark, Landmark, Package, Review, Message, Notification, Payment

---

### 06 — Infrastructure — Repository + UnitOfWork ✅
- [x] `GenericRepository<T>` — implements IRepository<T> with EF Core
- [x] `ChatRepository` — custom queries للـ chat
- [x] `UnitOfWork` — Dictionary of repositories + SaveChangesAsync

---

### 07 — Infrastructure — Identity Setup ✅
- [x] `IdentitySeeder` — Seed Roles: Tourist, Guide, Admin
- [x] Default Admin: admin@tourguide.com / Admin@123456

---

### 08 — Infrastructure — JWT Generation ✅
- [x] `JwtSettings` — Key, Issuer, Audience, AccessTokenExpiryMinutes (15), RefreshTokenExpiryDays (7)
- [x] `JwtService : IJwtService`
  - `GenerateAccessTokenAsync` — claims: UserId, Email, Role, FullName, IsApproved
  - `GenerateRefreshToken` — random base64 + expiry

---

### 09 — Infrastructure — Email Service ✅
- [x] `EmailSettings` — Host, Port, Username, Password, From
- [x] `EmailService : IEmailService` — MailKit SMTP + StartTls
- [x] Templates: Confirmation, Reset Password, Guide Rejection, Guide Approval
- [x] Gmail configured ✅ — tested ✅

---

### 10 — Infrastructure — Cloudinary Service ✅
- [x] `CloudinarySettings` — CloudName, ApiKey, ApiSecret
- [x] `CloudinaryService : ICloudinaryService`
  - `UploadImageAsync` — IFormFile + folder + UseFilename + UniqueFilename
  - `DeleteImageAsync` — extract publicId from URL + destroy
- [x] Cloudinary configured ✅ — tested ✅ (avatar upload working)

---

### 11 — Infrastructure — Paymob Service ✅
- [x] `PaymobSettings` — ApiKey, IntegrationId, IframeId, HmacSecret
- [x] `PaymobService`
  - `GetAuthTokenAsync`
  - `CreateOrderAsync` — amount in cents + currency
  - `GetPaymentKeyAsync` — auth token + order + billing data
  - `ValidateHmac` — HMAC-SHA512 webhook validation
- [x] Paymob Sandbox configured ✅ — Integration: 5853399, Iframe: 1069052
- [ ] ⏳ Webhook test with ngrok

---

### 12 — Infrastructure — SignalR Hubs ✅
- [x] `ChatHub`
  - `OnConnectedAsync` — add to personal group + booking groups
  - `OnDisconnectedAsync` — broadcast UserOffline
  - `SendMessage` — save to DB + push to booking group
  - `MarkAsRead` — set IsRead = true
- [x] `NotificationHub` — push via `IHubContext<NotificationHub>`
- [x] Mapped: `/hubs/chat` + `/hubs/notifications`

---

### 13 — Auth ✅
- [x] DTOs: RegisterRequest, LoginRequest, LoginResponse, GoogleAuthRequest, ForgetPasswordRequest, ResetPasswordRequest, RefreshTokenRequest, ChangePasswordRequest
- [x] `IAuthService` interface
- [x] `AuthService` — Register, Login, GoogleLogin, ConfirmEmail, ForgetPassword, ResetPassword, RefreshToken, Logout, ChangePassword
- [x] `AuthController` — 9 endpoints
- [x] Tested ✅ — Register + Email Confirmation + Login working

---

### 14 — User ✅
- [x] DTOs: UserDto, UpdateProfileRequest, AvatarResponse, PaginatedUsersRequest
- [x] `IUserService`, `UserService`
- [x] `UsersController` — GET /api/users/me, PUT /api/users/me, PUT /api/users/me/avatar
- [x] Avatar upload to Cloudinary tested ✅

---

### 15 — Cities & Landmarks ✅
- [x] DTOs: CityDto, CreateCityRequest, UpdateCityRequest, LandmarkDto, CreateLandmarkRequest, UpdateLandmarkRequest, LandmarkFilterParams
- [x] `ICityService`, `CityService`, `CitiesController` — 6 endpoints (CRUD + trending)
- [x] `ILandmarkService`, `LandmarkService`, `LandmarksController` — 7 endpoints (CRUD + images)
- [x] Filter params: cityId, category, minRating, maxPrice, search, sortBy, sortDir, page, pageSize

---

### 16 — Guide ✅
- [x] DTOs: GuideProfileDto, UpdateGuideRequest, GuideListDto
- [x] `IGuideService`, `GuideService`
- [x] `GuidesController` — 4 endpoints (me, update, public profile, all guides)
- [x] Admin endpoints (pending, approve, reject, suspend) → AdminController

---

### 17 — Packages ✅
- [x] DTOs: PackageDto, CreatePackageRequest, UpdatePackageRequest, AddLandmarkToPackage, PackageFilterParams
- [x] `IPackageService`, `PackageService`
- [x] `PackagesController` — 11 endpoints (CRUD + toggle + landmarks + images + compare)

---

### 18 — Custom Trip ✅
- [x] DTOs: CalculatePriceRequest, CalculatePriceResponse, AvailableGuidesRequest, CreateCustomTripRequest
- [x] `ICustomTripService`, `CustomTripService`
- [x] `CustomTripsController` — 3 endpoints (calculate, available-guides, create)
- [x] Price = Sum(landmark.EntryFee) × numberOfPersons × durationMultiplier + guideFixedFee

---

### 19 — Bookings ✅
- [x] DTOs: BookingDto, CreateBookingRequest, RejectBookingRequest, BookingFilterParams
- [x] `IBookingService`, `BookingService`
- [x] `BookingsController` — 9 endpoints
- [x] Status flow: Pending → Confirmed/Rejected/Cancelled → Completed

---

### 20 — Payment ✅
- [x] DTOs: InitiatePaymentRequest, InitiatePaymentResponse, PaymentStatusDto, PaymobWebhookDto
- [x] `IPaymentService`, `IPaymobService`, `PaymentService`
- [x] `PaymentsController` — 3 endpoints (initiate, webhook, status)
- [x] 10-step Paymob flow implemented
- [x] HMAC-SHA512 webhook validation
- [ ] ⏳ End-to-end test with ngrok

---

### 21 — Chat ✅
- [x] DTOs: MessageDto, ConversationDto, SendMessageRequest
- [x] `IChatRepository`, `ChatRepository`, `IChatService`, `ChatService`
- [x] `ChatController` — 3 endpoints (conversations, messages, unread-count)
- [x] SignalR: per-booking groups, real-time push, presence

---

### 22 — Reviews ✅
- [x] DTOs: CreateReviewRequest, UpdateReviewRequest, ReviewDto
- [x] `IReviewService`, `ReviewService`
- [x] `ReviewsController` — 4 endpoints
- [x] Business rules: only after Completed booking, one review per booking
- [x] Auto-recalculate GuideProfile.AverageRating on every change

---

### 23 — Notifications ✅
- [x] DTOs: NotificationDto
- [x] `INotificationService`, `NotificationService`
- [x] `NotificationsController` — 4 endpoints (list, mark-read, mark-all-read, count)
- [x] Real-time push via IHubContext<NotificationHub>
- [x] Triggers: NewBooking, BookingAccepted, BookingRejected, PaymentConfirmed, NewMessage, GuideApproved, TripReminder

---

### 24 — Admin Dashboard ✅
- [x] DTOs: DashboardSummaryDto, BookingsReportDto, RevenueReportDto, TopCityDto, TopLandmarkDto, GuidePerformanceDto, UserGrowthDto
- [x] `IAdminService`, `AdminService`
- [x] `AdminController` — 15 endpoints:
  - GET /api/admin/dashboard
  - GET /api/admin/reports/bookings, revenue, top-cities, top-landmarks, guides, users
  - GET/PUT /api/admin/guides/pending, approve, reject, suspend
  - GET/PUT /api/admin/users, users/{id}/ban
  - GET/DELETE /api/admin/reviews, reviews/{id}

---

### 25 — API — Global Exception Middleware ✅
- [x] `GlobalExceptionHandler` — maps Domain exceptions to HTTP status codes
- [x] `RequestLoggingMiddleware` — logs method, path, status code, duration

---

### 26 — API — DI Registration + Program.cs ✅
- [x] `ServiceCollectionExtensions` — AddDatabase, AddIdentityConfig, AddSettings, AddInfrastructureServices, AddRepositories, AddAuthServices, AddApplicationServices
- [x] JWT Bearer Authentication
- [x] CORS — AllowCredentials (for SignalR)
- [x] SignalR — AddSignalR() + MapHub<ChatHub> + MapHub<NotificationHub>
- [x] Swagger + JWT Bearer button
- [x] IdentitySeeder on startup

---

### 27 — API — Swagger + JWT + CORS ✅
- [x] Swagger JWT Bearer button (Authorize)
- [x] CORS — WithOrigins("http://localhost:4200") + AllowCredentials
- [x] All endpoints visible in Swagger UI ✅

---

### 28 — EF Core Migrations ✅
- [x] `InitialCreate` migration
- [x] `Update-Database` — TourGuideDb created on DESKTOP-09JL97C\SQLEXPRESS
- [x] Seed: Tourist, Guide, Admin roles + default Admin user

---

### 29 — Testing & Verification 🔄
- [x] Register + Email Confirmation — tested ✅
- [x] Login (Tourist + Admin) — tested ✅
- [x] Avatar Upload (Cloudinary) — tested ✅
- [ ] ⏳ Paymob payment flow — needs ngrok for webhook
- [ ] SignalR Chat — manual test pending
- [ ] SignalR Notifications — manual test pending
- [ ] Google OAuth login — needs frontend

---

## 🔑 appsettings.json structure

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "JWT": {
    "Key": "...",
    "Issuer": "TourGuideAPI",
    "Audience": "TourGuideClient",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  },
  "Google": { "ClientId": "..." },
  "Cloudinary": { "CloudName": "dp1po0xxf", "ApiKey": "...", "ApiSecret": "..." },
  "Paymob": { "ApiKey": "...", "IntegrationId": "5853399", "IframeId": "1069052", "HmacSecret": "..." },
  "Email": { "Host": "smtp.gmail.com", "Port": 587, "Username": "...", "Password": "...", "From": "..." }
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
| ⏳ | Pending |