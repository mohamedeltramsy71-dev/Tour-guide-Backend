# Smart Tour Guide — Backend Progress Tracker
> ASP.NET Core 8 | Clean Architecture | SQL Server | SignalR | Paymob | JWT + Google OAuth

---

## 🌐 Server URL
> **https://tourguidee.runasp.net**
> Swagger UI: https://tourguidee.runasp.net/swagger/index.html

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
│   │   ├── Landmark.cs               ✅ (Category → string)
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
│   │   ├── IRepository.cs            ✅ (+ Include methods)
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
│   │   │   ├── LoginResponse.cs         ✅ (+ UserId + AvatarUrl)
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
│   │   │   └── GuideListDto.cs          ✅ (+ GuideProfileId = g.Id)
│   │   ├── Package
│   │   │   ├── PackageDto.cs               ✅ (Images → List<PackageImageDto> {Id, ImageUrl} + GuideProfileId)
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
│   │   │   └── BookingFilterParams.cs      ✅ (Status → string not enum)
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
│   │   │   └── ReviewDto.cs                ✅ (+ GuideName)
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
│   │   ├── IEmailService.cs          ✅ (+ SendNotificationEmailAsync + SendNewMessageEmailAsync)
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
│   │   ├── IChatService.cs           ✅ (+ MarkMessagesAsReadAsync)
│   │   ├── IReviewService.cs         ✅
│   │   ├── INotificationService.cs   ✅
│   │   ├── INotificationPushService.cs ✅ ← NEW
│   │   └── IAdminService.cs          ✅
│   ├── Services
│   │   ├── AuthService.cs            ✅ (response wrapped {message, data} + Frontend links + UserId + AvatarUrl)
│   │   ├── UserService.cs            ✅
│   │   ├── CityService.cs            ✅
│   │   ├── LandmarkService.cs        ✅ (Category enum → string)
│   │   ├── GuideService.cs           ✅
│   │   ├── PackageService.cs         ✅ (Images → PackageImageDto + GuideProfileId)
│   │   ├── CustomTripService.cs      ✅ (FindWithNestedIncludeAsync + GuideProfileId fix)
│   │   ├── BookingService.cs         ✅ (Include Tourist + Guide + Package + Notifications)
│   │   ├── PaymentService.cs         ✅
│   │   ├── ChatService.cs            ✅ (+ MarkMessagesAsReadAsync)
│   │   ├── ReviewService.cs          ✅ (Include Tourist + GuideProfile)
│   │   ├── NotificationService.cs    ✅ (Save DB + SignalR push + Email fire-and-forget)
│   │   └── AdminService.cs           ✅ (Bug Fix: TopCities + GuideName)
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
│   │   ├── GenericRepository.cs      ✅ (+ Include methods)
│   │   ├── UnitOfWork.cs             ✅
│   │   └── ChatRepository.cs         ✅ (+ OrderBy fix + Include GuideProfile)
│   ├── Services
│   │   ├── EmailSettings.cs          ✅
│   │   ├── EmailService.cs           ✅ (+ SendNotificationEmailAsync + SendNewMessageEmailAsync)
│   │   ├── CloudinarySettings.cs     ✅
│   │   ├── CloudinaryService.cs      ✅
│   │   ├── PaymobSettings.cs         ✅
│   │   ├── PaymobService.cs          ✅
│   │   └── NotificationPushService.cs ✅ ← NEW (IHubContext<NotificationHub>)
│   ├── Identity
│   │   ├── JwtSettings.cs            ✅
│   │   ├── JwtService.cs             ✅
│   │   └── IdentitySeeder.cs         ✅
│   └── Hubs
│       ├── ChatHub.cs                ✅ (+ JoinBookingGroup + SenderName + Email on message)
│       └── NotificationHub.cs        ✅
│
└── TourGuide.API
    ├── Controllers
    │   ├── AuthController.cs              ✅ (9 endpoints — response {message,data} + Frontend links)
    │   ├── UsersController.cs             ✅ (3 endpoints)
    │   ├── CitiesController.cs            ✅ (6 endpoints + POST /upload-image)
    │   ├── LandmarksController.cs         ✅ (7 endpoints)
    │   ├── GuidesController.cs            ✅ (4 endpoints)
    │   ├── PackagesController.cs          ✅ (11 endpoints)
    │   ├── CustomTripsController.cs       ✅ (3 endpoints)
    │   ├── BookingsController.cs          ✅ (9 endpoints)
    │   ├── PaymentsController.cs          ✅ (3 endpoints)
    │   ├── ChatController.cs              ✅ (4 endpoints + PUT /{bookingId}/read)
    │   ├── ReviewsController.cs           ✅ (4 endpoints)
    │   ├── NotificationsController.cs     ✅ (4 endpoints)
    │   └── AdminController.cs             ✅ (17 endpoints)
    ├── Extensions
    │   └── ServiceCollectionExtensions.cs ✅ (+ INotificationPushService registered)
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

## ✅ Progress Overview — ALL DONE 🎉

| # | Module | Status | Notes |
|---|--------|--------|-------|
| 00 | Solution & Project Setup | ✅ Done | |
| 01 | Domain — Entities | ✅ Done | 15 entities |
| 02 | Domain — Enums | ✅ Done | 5 enums |
| 03 | Domain — Interfaces | ✅ Done | IRepository (+ Include methods), IUnitOfWork, IChatRepository |
| 04 | Domain — Exceptions | ✅ Done | 4 exceptions |
| 05 | Infrastructure — AppDbContext + Configurations | ✅ Done | 9 configurations |
| 06 | Infrastructure — Generic Repository + UnitOfWork | ✅ Done | + Include methods |
| 07 | Infrastructure — Identity Setup | ✅ Done | Roles + Admin seeder |
| 08 | Infrastructure — JWT Generation | ✅ Done | implements IJwtService |
| 09 | Infrastructure — Email Service | ✅ Done | Gmail SMTP ✅ + SendNotificationEmailAsync + SendNewMessageEmailAsync |
| 10 | Infrastructure — Cloudinary Service | ✅ Done | Cloudinary keys ✅ |
| 11 | Infrastructure — Paymob Service | ✅ Done | Paymob Sandbox keys ✅ |
| 12 | Infrastructure — SignalR Hubs | ✅ Done | ChatHub + NotificationHub |
| 12b | Infrastructure — NotificationPushService | ✅ Done | ← NEW — IHubContext push |
| 13 | Auth | ✅ Done | 9 endpoints — response {message,data} — Frontend links |
| 14 | User | ✅ Done | 3 endpoints — Avatar Upload ✅ |
| 15 | Cities & Landmarks | ✅ Done | 6+7 endpoints + cities/upload-image |
| 16 | Guide | ✅ Done | 4 endpoints |
| 17 | Packages | ✅ Done | 11 endpoints — Images → PackageImageDto + GuideProfileId |
| 18 | Custom Trip | ✅ Done | 3 endpoints — FindWithNestedIncludeAsync fix ✅ |
| 19 | Bookings | ✅ Done | 9 endpoints — Includes + Notifications + enum fix |
| 20 | Payment | ✅ Done | 3 endpoints — Paymob Approved ✅ — Webhook E2E tested ✅ |
| 21 | Chat | ✅ Done | 4 endpoints + SignalR Hub — tested with Frontend ✅ |
| 22 | Reviews | ✅ Done | 4 endpoints + Include Tourist + GuideProfile + GuideName |
| 23 | Notifications | ✅ Done | 4 endpoints — DB + SignalR push + Email — tested with Frontend ✅ |
| 24 | Admin Dashboard | ✅ Done | 17 endpoints — Bug Fix: TopCities + GuideName + GET/DELETE users/{id} ✅ |
| 25 | API — Global Exception Middleware | ✅ Done | |
| 26 | API — DI Registration + Program.cs | ✅ Done | |
| 27 | API — Swagger + JWT + CORS | ✅ Done | |
| 28 | EF Core — Migrations + Seed Data | ✅ Done | + AddCategoriesTable + ChangeLandmarkCategoryToString + Bookings enum SQL fix |
| 29 | Testing & Verification | ✅ Done | كل الـ APIs متيستة مع الـ Frontend ✅ |
| 30 | Deployment | ✅ Done | https://tourguidee.runasp.net ✅ |
| 31 | Paymob Webhook Config | ✅ Done | URL set ✅ — E2E tested ✅ |
| 32 | Google OAuth Config | ✅ Done | Origins + Redirect URIs ✅ — E2E tested with Frontend ✅ |
| 33 | SignalR Chat Test | ✅ Done | tested with Frontend ✅ |
| 34 | SignalR Notifications Test | ✅ Done | tested with Frontend ✅ |
| 35 | Google OAuth E2E Test | ✅ Done | tested with Frontend ✅ |
| 36 | Paymob Webhook E2E Test | ✅ Done | tested ✅ |
| 37 | CORS — Production Frontend URL | ⏳ Pending | لما الـ Frontend يتحط على Vercel |

---

## 🔧 Backend Fixes Log

| Fix | File(s) |
|-----|---------|
| Auth response wrapped `{ message, data }` | `AuthController.cs` |
| Reset/Confirm Email links → Frontend URL | `AuthController.cs` / `AuthService.cs` |
| LoginResponse.UserId + AvatarUrl added | `LoginResponse.cs` |
| AuthService — UserId + AvatarUrl في كل new LoginResponse | `AuthService.cs` |
| IRepository + GenericRepository — Include methods added | `IRepository.cs` + `GenericRepository.cs` |
| IRepository.FindWithNestedIncludeAsync → object? (nullable fix) | `IRepository.cs` + `GenericRepository.cs` |
| ReviewService — Include Tourist + GuideProfile | `ReviewService.cs` |
| ReviewDto — added GuideName | `ReviewDto.cs` |
| BookingService — Include Tourist + Guide + Package | `BookingService.cs` |
| BookingService — .Include(b => b.Package) removed cast | `BookingService.cs` |
| BookingFilterParams.Status — string not enum | `BookingFilterParams.cs` |
| CitiesController — POST `/api/cities/upload-image` added | `CitiesController.cs` |
| Landmark.Category — enum → string | `Landmark.cs` + `LandmarkService.cs` |
| Migration: AddCategoriesTable | `TourGuide.Infrastructure` |
| Migration: ChangeLandmarkCategoryToString | `TourGuide.Infrastructure` |
| Bookings BookingStatus enum fix (string → int in DB) | SQL UPDATE |
| PackageDto.Images → `List<PackageImageDto> { Id, ImageUrl }` | `PackageDto.cs` + `PackageService.cs` |
| PackageDto.GuideProfileId added | `PackageDto.cs` + `PackageService.cs` |
| INotificationPushService interface added | `INotificationPushService.cs` ← NEW |
| NotificationPushService implementation added | `NotificationPushService.cs` ← NEW |
| NotificationService — SignalR push + Email fire-and-forget | `NotificationService.cs` |
| IEmailService — SendNotificationEmailAsync added | `IEmailService.cs` |
| EmailService — SendNotificationEmailAsync HTML template | `EmailService.cs` |
| IEmailService — SendNewMessageEmailAsync added | `IEmailService.cs` ← NEW |
| EmailService — SendNewMessageEmailAsync implemented | `EmailService.cs` ← NEW |
| ServiceCollectionExtensions — INotificationPushService registered | `ServiceCollectionExtensions.cs` |
| ChatHub.OnConnectedAsync — user_{userId} group فقط (شيل DB query) | `ChatHub.cs` ← NEW |
| ChatHub.JoinBookingGroup — method جديدة بيناديها الـ Frontend | `ChatHub.cs` ← NEW |
| ChatHub.SendMessage — تحقق participant + SenderName + ISO CreatedAt + Email | `ChatHub.cs` ← NEW |
| ChatHub — inject IEmailService + UserManager | `ChatHub.cs` ← NEW |
| IChatService.MarkMessagesAsReadAsync added | `IChatService.cs` ← NEW |
| ChatService.MarkMessagesAsReadAsync implemented | `ChatService.cs` ← NEW |
| ChatController — PUT `/{bookingId}/read` endpoint added | `ChatController.cs` ← NEW |
| ChatRepository.GetMessagesAsync — OrderBy بدل OrderByDescending | `ChatRepository.cs` ← NEW |
| ChatRepository.GetBookingWithGuideAsync — أضاف Include(GuideProfile) | `ChatRepository.cs` ← NEW |
| PaymentService + PaymobService (3-step flow) | `PaymentService.cs` + `PaymobService.cs` |
| PaymentsController — initiate + webhook + status | `PaymentsController.cs` |
| Payment + PaymentStatus entities | `Payment.cs` |
| CustomTripService.GetAvailableGuidesAsync — استخدام FindWithNestedIncludeAsync | `CustomTripService.cs` ← NEW |
| GuideListDto — GuideProfileId = g.Id added | `GuideListDto.cs` + `CustomTripService.cs` ← NEW |
| AdminService — Bug Fix: TopCities NullReference على p.City | `AdminService.cs` |
| AdminService — Bug Fix: GuideName join مع UserManager | `AdminService.cs` |
| AdminController — GET + DELETE `/api/admin/users/{id}` added | `AdminController.cs` |

---

## 🔑 External Services Config

| Service | Status | Notes |
|---------|--------|-------|
| Gmail SMTP | ✅ Done | mohamedeltramsy71@gmail.com — tested ✅ |
| Cloudinary | ✅ Done | Cloud: dp1po0xxf — tested ✅ |
| Google OAuth | ✅ Done | Origins + Redirect URIs configured ✅ — E2E tested with Frontend ✅ |
| Paymob Sandbox | ✅ Done | Integration: 5853399 — Iframe: 1069052 — Webhook URL set ✅ — E2E tested ✅ |

---

## 📋 Detailed Steps Log

### 01 — Domain — Entities ✅
- [x] `ApplicationUser` (extends IdentityUser) — FullName, Bio, Phone, AvatarUrl, IsDeleted, IsBanned
- [x] `RefreshToken` — Token, ExpiresAt, IsRevoked, UserId
- [x] `GuideProfile` — Bio, ExperienceYears, AverageRating, IsApproved, IsSuspended, LanguagesJson
- [x] `City` — NameAr, NameEn, Description, ImageUrl, IsDeleted
- [x] `GuideCity` — composite key (GuideProfileId, CityId)
- [x] `Landmark` — NameAr, NameEn, Category (string), EntryFee, CityId, IsDeleted
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
- [x] `IRepository<T>` — GetById, GetAll, Find, FindOne, Add, Update, Delete, Exists, Count + **Include methods**
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
- [x] `GenericRepository<T>` — implements IRepository<T> with EF Core + **Include methods**
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
- [x] **`SendNotificationEmailAsync`** — HTML template with icon per NotificationType ← NEW
- [x] **`SendNewMessageEmailAsync`** — email on new chat message with link to /chat ← NEW
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
- [x] Payment Approved ✅ — tested with 3DS card
- [x] Webhook URL set in Paymob Dashboard ✅ → https://tourguidee.runasp.net/api/payments/webhook
- [x] Webhook E2E tested ✅

---

### 12 — Infrastructure — SignalR Hubs ✅
- [x] `ChatHub`
  - `OnConnectedAsync` — add to user_{userId} personal group only (no DB query) ← Fix
  - `OnDisconnectedAsync` — broadcast UserOffline
  - `JoinBookingGroup` — new method called by Frontend on selectConversation ← NEW
  - `SendMessage` — verify participant + SenderName + ISO CreatedAt + Email ← Fix
  - `MarkAsRead` — set IsRead = true
- [x] `NotificationHub` — push via `IHubContext<NotificationHub>`
- [x] `NotificationPushService : INotificationPushService` — **← NEW**
  - Pushes `NotificationReceived` event to `user_{userId}` group
- [x] Mapped: `/hubs/chat` + `/hubs/notifications`
- [x] SignalR Chat tested with Frontend ✅
- [x] SignalR Notifications tested with Frontend ✅

---

### 13 — Auth ✅
- [x] DTOs: RegisterRequest, LoginRequest, LoginResponse, GoogleAuthRequest, ForgetPasswordRequest, ResetPasswordRequest, RefreshTokenRequest, ChangePasswordRequest
- [x] `IAuthService` interface
- [x] `AuthService` — Register, Login, GoogleLogin, ConfirmEmail, ForgetPassword, ResetPassword, RefreshToken, Logout, ChangePassword
- [x] `AuthController` — 9 endpoints
- [x] **Response wrapped `{ message, data }`** ← Fix
- [x] **Reset/Confirm Email links → Frontend URL** ← Fix
- [x] **LoginResponse.UserId + AvatarUrl added** ← Fix
- [x] Tested ✅ — Register + Email Confirmation + Login + Google OAuth all working

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
- [x] **POST `/api/cities/upload-image`** ← Fix (city image upload)
- [x] `ILandmarkService`, `LandmarkService`, `LandmarksController` — 7 endpoints (CRUD + images)
- [x] **Landmark.Category — enum → string** ← Fix
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
- [x] **PackageDto.Images → `List<PackageImageDto> { Id, ImageUrl }`** ← Fix
- [x] **PackageDto.GuideProfileId added** ← Fix
- [x] `IPackageService`, `PackageService`
- [x] `PackagesController` — 11 endpoints (CRUD + toggle + landmarks + images + compare)

---

### 18 — Custom Trip ✅
- [x] DTOs: CalculatePriceRequest, CalculatePriceResponse, AvailableGuidesRequest, CreateCustomTripRequest
- [x] `ICustomTripService`, `CustomTripService`
- [x] `CustomTripsController` — 3 endpoints (calculate, available-guides, create)
- [x] Price = Sum(landmark.EntryFee) × numberOfPersons × durationMultiplier + guideFixedFee
- [x] **Fix: `GetAvailableGuidesAsync` → استخدم `FindWithNestedIncludeAsync`** ← Fix
  - `.Include(g => g.User).Include(g => g.CoveredCities).ThenInclude(gc => gc.City)`
  - Filter الـ CityId بيتعمل in-memory بعد الـ Include
- [x] **Fix: `GuideListDto.GuideProfileId = g.Id` added** ← Fix
- [x] Tested with Frontend ✅

---

### 19 — Bookings ✅
- [x] DTOs: BookingDto, CreateBookingRequest, RejectBookingRequest, BookingFilterParams
- [x] `IBookingService`, `BookingService`
- [x] `BookingsController` — 9 endpoints
- [x] Status flow: Pending → Confirmed/Rejected/Cancelled → Completed
- [x] **Include Tourist + Guide + Package** ← Fix
- [x] **BookingFilterParams.Status — string not enum** ← Fix
- [x] Bug Fix: `GuideProfileId` بيتحط أوتوماتيك من الـ Package ✅
- [x] Bug Fix: Endpoint اسمه `/accept` مش `/confirm` ✅
- [x] Notifications triggered on every action ✅

---

### 20 — Payment ✅
- [x] DTOs: InitiatePaymentRequest, InitiatePaymentResponse, PaymentStatusDto, PaymobWebhookDto
- [x] `IPaymentService`, `IPaymobService`, `PaymentService`
- [x] `PaymentsController` — 3 endpoints (initiate, webhook, status)
- [x] 10-step Paymob flow implemented
- [x] HMAC-SHA512 webhook validation
- [x] Payment Approved ✅ — tested with 3DS card
- [x] Webhook URL set in Paymob Dashboard ✅
- [x] Webhook E2E tested ✅

---

### 21 — Chat ✅
- [x] DTOs: MessageDto, ConversationDto, SendMessageRequest
- [x] `IChatRepository`, `ChatRepository`, `IChatService`, `ChatService`
- [x] `ChatController` — 4 endpoints (conversations, messages, unread-count, **PUT /{bookingId}/read**)
- [x] **Fix: ChatHub.OnConnectedAsync** — user_{userId} group فقط ← Fix
- [x] **Fix: ChatHub.JoinBookingGroup** — method جديدة بيناديها الـ Frontend ← Fix
- [x] **Fix: ChatHub.SendMessage** — participant check + SenderName + ISO CreatedAt + Email ← Fix
- [x] **Fix: IChatService + ChatService.MarkMessagesAsReadAsync** ← Fix
- [x] **Fix: ChatController PUT /{bookingId}/read** ← Fix
- [x] **Fix: ChatRepository.GetMessagesAsync** — OrderBy بدل OrderByDescending ← Fix
- [x] **Fix: ChatRepository.GetBookingWithGuideAsync** — Include(GuideProfile) ← Fix
- [x] **Fix: IEmailService.SendNewMessageEmailAsync** — email on new message ← Fix
- [x] SignalR: per-booking groups, real-time push, presence, onreconnected support
- [x] Tested with Frontend ✅

---

### 22 — Reviews ✅
- [x] DTOs: CreateReviewRequest, UpdateReviewRequest, ReviewDto
- [x] **ReviewDto.GuideName added** ← Fix
- [x] `IReviewService`, `ReviewService`
- [x] **Include Tourist + GuideProfile** ← Fix
- [x] `ReviewsController` — 4 endpoints
- [x] Business rules: only after Completed booking, one review per booking
- [x] Auto-recalculate GuideProfile.AverageRating on every change

---

### 23 — Notifications ✅
- [x] DTOs: NotificationDto
- [x] `INotificationService`, `NotificationService`
- [x] **`INotificationPushService` interface** ← NEW
- [x] **`NotificationPushService`** — push via IHubContext ← NEW
- [x] `NotificationsController` — 4 endpoints (list, mark-read, mark-all-read, count)
- [x] **Flow: Save DB → SignalR push → Email fire-and-forget** ← NEW
- [x] Triggers: NewBooking, BookingAccepted, BookingRejected, PaymentConfirmed, NewMessage, GuideApproved, TripReminder
- [x] Tested with Frontend ✅ — Notifications وصلت real-time + Email

---

### 24 — Admin Dashboard ✅
- [x] DTOs: DashboardSummaryDto, BookingsReportDto, RevenueReportDto, TopCityDto, TopLandmarkDto, GuidePerformanceDto, UserGrowthDto
- [x] `IAdminService`, `AdminService`
- [x] `AdminController` — 17 endpoints:
  - GET /api/admin/dashboard
  - GET /api/admin/reports/bookings, revenue, top-cities, top-landmarks, guides, users
  - GET/PUT /api/admin/guides/pending, approve, reject, suspend
  - GET/PUT/DELETE /api/admin/users, users/{id}, users/{id}/ban ← Bug Fix ✅
  - GET/DELETE /api/admin/reviews, reviews/{id}
- [x] Bug Fix: `GetTopCitiesAsync` — NullReference على `p.City` → Join منفصل مع City repository ✅
- [x] Bug Fix: `GuideName = "Unknown"` → Join مع `_userManager.Users` ✅
- [x] Bug Fix: أضفنا `GET /api/admin/users/{id}` و `DELETE /api/admin/users/{id}` ✅

---

### 25 — API — Global Exception Middleware ✅
- [x] `GlobalExceptionHandler` — maps Domain exceptions to HTTP status codes
- [x] `RequestLoggingMiddleware` — logs method, path, status code, duration

---

### 26 — API — DI Registration + Program.cs ✅
- [x] `ServiceCollectionExtensions` — AddDatabase, AddIdentityConfig, AddSettings, AddInfrastructureServices, AddRepositories, AddAuthServices, AddApplicationServices
- [x] **`INotificationPushService` → `NotificationPushService` registered** ← NEW
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
- [x] `Update-Database` — TourGuideDb created
- [x] **`AddCategoriesTable` migration** ← Fix
- [x] **`ChangeLandmarkCategoryToString` migration** ← Fix
- [x] **Bookings BookingStatus — SQL UPDATE fix (string → int)** ← Fix
- [x] Seed: Tourist, Guide, Admin roles + default Admin user

---

### 29 — Testing & Verification ✅
- [x] Register + Email Confirmation — tested ✅
- [x] Login (Tourist + Guide + Admin) — tested ✅
- [x] Google OAuth E2E — tested with Frontend ✅
- [x] Avatar Upload (Cloudinary) — tested ✅
- [x] Cities & Landmarks CRUD — tested ✅
- [x] Guide Profile Update + Approve — tested ✅
- [x] Packages CRUD — tested ✅
- [x] Create Booking (Tourist) — tested ✅
- [x] Accept + Complete Booking (Guide) — tested ✅
- [x] Initiate Payment (Paymob) — Approved ✅
- [x] Paymob Webhook E2E — tested ✅
- [x] Create Review — tested ✅
- [x] Notifications — DB + SignalR + Email — tested with Frontend ✅
- [x] SignalR Chat — real-time messages — tested with Frontend ✅
- [x] Custom Trip — calculate + available-guides + create — tested ✅
- [x] Admin Dashboard + Reports — tested ✅

---

### 30 — Deployment ✅
- [x] رفع الـ API على runasp.net ✅
- [x] Connection String على Production DB ✅
- [x] CORS يقبل http://localhost:4200 ✅
- [x] appsettings على السيرفر ✅
- [x] Swagger شغال على Production ✅
- [ ] ⏳ CORS يقبل Frontend Vercel URL — لما الـ Frontend يتحط على Vercel

---

### 31 — Paymob Webhook Config ✅
- [x] Webhook URL حُط في Paymob Dashboard ✅
  - URL: https://tourguidee.runasp.net/api/payments/webhook
  - Integration ID: 5853399
- [x] Webhook E2E tested ✅

---

### 32 — Google OAuth Config ✅
- [x] Authorized JavaScript Origins:
  - http://localhost:4200
  - https://tourguidee.runasp.net
- [x] Authorized Redirect URIs:
  - http://localhost:4200
  - https://tourguidee.runasp.net/signin-google
- [x] E2E tested with Frontend ✅

---

## ⚠️ Production Checklist (لما الـ Frontend يتحط على Vercel)

```csharp
// Program.cs CORS — أضف Vercel URL
WithOrigins("http://localhost:4200", "https://your-app.vercel.app")
```

```json
// appsettings.json
"Frontend": { "BaseUrl": "https://your-app.vercel.app" }
```

```
// Google Console
Authorized JavaScript origins: add Vercel URL
Authorized redirect URIs:      add Vercel URL/signin-google
```

```
// Paymob Dashboard → Response URL
https://your-app.vercel.app/payment/callback
```

```csharp
// EmailService.cs → SendNewMessageEmailAsync
// غير href="http://localhost:4200/chat" → href="https://your-app.vercel.app/chat"
```

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
| ⏳ | Pending — بعد الرفع / الـ Frontend |

---

> 🎉 **Backend Complete — 36/37 done! الـ 37 بس CORS بتاع Vercel لما الـ Frontend يتنشر**