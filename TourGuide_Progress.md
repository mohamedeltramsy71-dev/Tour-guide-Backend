# Smart Tour Guide — Backend Progress Tracker
> ASP.NET Core 8 | Clean Architecture | SQL Server | SignalR | Paymob | JWT + Google OAuth

---

## 🌐 Server URL
> **https://tourguidee.runasp.net**
> Swagger UI: https://tourguidee.runasp.net/swagger/index.html

## 🌐 Frontend URL (Production)
> **https://tour-guide-frontend-sable.vercel.app/**

---

## 🔑 Super Admin Credentials
```
Email    : admin@tourguide.com
Password : Admin@123456
```

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
│   │   ├── INotificationPushService.cs ✅
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
│   │   ├── EmailService.cs           ✅ (+ IConfiguration + _frontendBaseUrl + SendNotificationEmailAsync + SendNewMessageEmailAsync)
│   │   ├── CloudinarySettings.cs     ✅
│   │   ├── CloudinaryService.cs      ✅
│   │   ├── PaymobSettings.cs         ✅
│   │   ├── PaymobService.cs          ✅
│   │   └── NotificationPushService.cs ✅ (IHubContext<NotificationHub>)
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
        ├── CORS (AllowCredentials)        ✅ (+ Vercel URL)
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
| 09 | Infrastructure — Email Service | ✅ Done | Gmail SMTP ✅ + SendNotificationEmailAsync + SendNewMessageEmailAsync + IConfiguration _frontendBaseUrl |
| 10 | Infrastructure — Cloudinary Service | ✅ Done | Cloudinary keys ✅ |
| 11 | Infrastructure — Paymob Service | ✅ Done | Paymob Sandbox keys ✅ |
| 12 | Infrastructure — SignalR Hubs | ✅ Done | ChatHub + NotificationHub |
| 12b | Infrastructure — NotificationPushService | ✅ Done | IHubContext push |
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
| 37 | CORS — Production Frontend URL | ✅ Done | Vercel URL added ✅ |
| 38 | appsettings.json — Frontend BaseUrl | ✅ Done | Vercel URL + appsettings.Development.json للـ localhost |
| 39 | EmailService — IConfiguration _frontendBaseUrl | ✅ Done | dynamic URL من appsettings بدل hardcoded localhost |
| 40 | Google Console — Vercel URL | ✅ Done | Origins + Redirect URIs updated ✅ |
| 41 | Paymob Dashboard — Response URL | ✅ Done | Vercel /payment/callback ✅ |

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
| INotificationPushService interface added | `INotificationPushService.cs` |
| NotificationPushService implementation added | `NotificationPushService.cs` |
| NotificationService — SignalR push + Email fire-and-forget | `NotificationService.cs` |
| IEmailService — SendNotificationEmailAsync added | `IEmailService.cs` |
| EmailService — SendNotificationEmailAsync HTML template | `EmailService.cs` |
| IEmailService — SendNewMessageEmailAsync added | `IEmailService.cs` |
| EmailService — SendNewMessageEmailAsync implemented | `EmailService.cs` |
| EmailService — IConfiguration inject + _frontendBaseUrl dynamic | `EmailService.cs` |
| ServiceCollectionExtensions — INotificationPushService registered | `ServiceCollectionExtensions.cs` |
| ChatHub.OnConnectedAsync — user_{userId} group فقط (شيل DB query) | `ChatHub.cs` |
| ChatHub.JoinBookingGroup — method جديدة بيناديها الـ Frontend | `ChatHub.cs` |
| ChatHub.SendMessage — تحقق participant + SenderName + ISO CreatedAt + Email | `ChatHub.cs` |
| ChatHub — inject IEmailService + UserManager | `ChatHub.cs` |
| IChatService.MarkMessagesAsReadAsync added | `IChatService.cs` |
| ChatService.MarkMessagesAsReadAsync implemented | `ChatService.cs` |
| ChatController — PUT `/{bookingId}/read` endpoint added | `ChatController.cs` |
| ChatRepository.GetMessagesAsync — OrderBy بدل OrderByDescending | `ChatRepository.cs` |
| ChatRepository.GetBookingWithGuideAsync — أضاف Include(GuideProfile) | `ChatRepository.cs` |
| PaymentService + PaymobService (3-step flow) | `PaymentService.cs` + `PaymobService.cs` |
| PaymentsController — initiate + webhook + status | `PaymentsController.cs` |
| Payment + PaymentStatus entities | `Payment.cs` |
| CustomTripService.GetAvailableGuidesAsync — استخدام FindWithNestedIncludeAsync | `CustomTripService.cs` |
| GuideListDto — GuideProfileId = g.Id added | `GuideListDto.cs` + `CustomTripService.cs` |
| AdminService — Bug Fix: TopCities NullReference على p.City | `AdminService.cs` |
| AdminService — Bug Fix: GuideName join مع UserManager | `AdminService.cs` |
| AdminController — GET + DELETE `/api/admin/users/{id}` added | `AdminController.cs` |
| Program.cs CORS — Vercel URL added | `Program.cs` |
| appsettings.json — Frontend BaseUrl → Vercel URL | `appsettings.json` |
| appsettings.Development.json — Frontend BaseUrl → localhost | `appsettings.Development.json` |
| Google Console — Vercel URL في Origins + Redirect URIs | Google Console |
| Paymob Dashboard — Response URL → Vercel /payment/callback | Paymob Dashboard |

---

## 🔑 External Services Config

| Service | Status | Notes |
|---------|--------|-------|
| Gmail SMTP | ✅ Done | mohamedeltramsy71@gmail.com — tested ✅ |
| Cloudinary | ✅ Done | Cloud: dp1po0xxf — tested ✅ |
| Google OAuth | ✅ Done | Origins + Redirect URIs configured ✅ + Vercel URL added ✅ |
| Paymob Sandbox | ✅ Done | Integration: 5853399 — Iframe: 1069052 — Webhook URL set ✅ — Response URL → Vercel ✅ |

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
  "Frontend": {
    "BaseUrl": "https://tour-guide-frontend-sable.vercel.app"
  },
  "Google": { "ClientId": "..." },
  "Cloudinary": { "CloudName": "dp1po0xxf", "ApiKey": "...", "ApiSecret": "..." },
  "Paymob": { "ApiKey": "...", "IntegrationId": "5853399", "IframeId": "1069052", "HmacSecret": "..." },
  "Email": { "Host": "smtp.gmail.com", "Port": 587, "Username": "...", "Password": "...", "From": "..." }
}
```

## 🔑 appsettings.Development.json structure

```json
{
  "Frontend": {
    "BaseUrl": "http://localhost:4200"
  }
}
```

---

## 🔑 Program.cs — CORS

```csharp
WithOrigins(
    "http://localhost:4200",
    "https://tour-guide-frontend-sable.vercel.app"
)
```

---

## 🔑 Google Console Config

**Authorized JavaScript Origins:**
```
http://localhost:4200
https://tourguidee.runasp.net
https://tour-guide-frontend-sable.vercel.app
```

**Authorized Redirect URIs:**
```
http://localhost:4200
https://tourguidee.runasp.net/signin-google
https://tour-guide-frontend-sable.vercel.app/signin-google
```

---

## 🔑 Paymob Dashboard Config

**Webhook URL:**
```
https://tourguidee.runasp.net/api/payments/webhook
```

**Response URL (Callback):**
```
https://tour-guide-frontend-sable.vercel.app/payment/callback
```

---

## 🧪 Test Data

### Paymob Test Card
```
Card Number : 5123456789012346
Expiry      : 12/27
CVV         : 123
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

---

> 🎉 **Backend Complete — 41/41 done! المشروع اتنشر بالكامل على Production ✅**
ENDOFFILE