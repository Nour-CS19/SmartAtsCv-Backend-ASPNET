# Smart ATS CV — ASP.NET Core Backend

باك إند كامل بـ ASP.NET Core 8 Web API + Entity Framework Core + SQL Server.

## المميزات
- تسجيل دخول / تسجيل مستخدمين (JWT Authentication + BCrypt لتشفير الباسورد)
- حفظ واسترجاع وتعديل وحذف السيرة الذاتية (CRUD كامل)، مربوط بكل مستخدم
- توليد PDF من السيرفر نفسه (QuestPDF) وتحميله مباشرة
- Swagger لتجربة الـ API فورًا

## خطوات التشغيل على Visual Studio

### 1. تأكد إن عندك:
- **Visual Studio 2022** (مع ورشة "ASP.NET and web development")
- **.NET 8 SDK** (بييجي مع Visual Studio عادةً، أو حمّله من موقع مايكروسوفت)
- **SQL Server** (Express أو Developer أو LocalDB) شغال عندك

### 2. افتح المشروع
افتح ملف `SmartAtsCv.Api.sln` بـ Visual Studio.

### 3. عدّل الاتصال بقاعدة البيانات
في ملف `SmartAtsCv.Api/appsettings.json`، غيّر `DefaultConnection` بحيث يطابق السيرفر بتاعك:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=SmartAtsCvDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

- لو بتستخدم **LocalDB**: `Server=(localdb)\\mssqllocaldb;Database=SmartAtsCvDb;Trusted_Connection=True;`
- لو عندك يوزر/باسورد بدل Windows Authentication: `Server=SERVER_NAME;Database=SmartAtsCvDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;`

### 4. غيّر مفتاح الـ JWT
في نفس الملف، غيّر `Jwt:Key` لأي نص عشوائي طويل (32 حرف على الأقل) قبل ما تنشر المشروع فعليًا.

### 5. أنشئ قاعدة البيانات (Migrations)
من **Package Manager Console** في Visual Studio (Tools > NuGet Package Manager > Package Manager Console):

```powershell
Add-Migration InitialCreate
Update-Database
```

ده هيعمل الجداول تلقائيًا (Users, CvProfiles, Experiences, Educations, Skills, Languages, Certifications).

### 6. شغّل المشروع
اضغط F5 أو زرار Run. هيفتح Swagger تلقائيًا على `/swagger` عشان تجرب الـ API.

## نقاط الوصول (Endpoints)

| Method | Route | الوظيفة | يحتاج تسجيل دخول |
|---|---|---|---|
| POST | `/api/auth/register` | تسجيل مستخدم جديد | لا |
| POST | `/api/auth/login` | تسجيل الدخول (يرجع Token) | لا |
| GET | `/api/cv` | كل السير الذاتية بتاعة المستخدم | نعم |
| GET | `/api/cv/{id}` | سيرة ذاتية واحدة | نعم |
| POST | `/api/cv` | إنشاء سيرة ذاتية جديدة | نعم |
| PUT | `/api/cv/{id}` | تعديل سيرة ذاتية | نعم |
| DELETE | `/api/cv/{id}` | حذف سيرة ذاتية | نعم |
| GET | `/api/cv/{id}/pdf` | تحميل السيرة كـ PDF من السيرفر | نعم |

بعد تسجيل الدخول أو التسجيل، هتاخد `token` — استخدمه في كل الطلبات اللي محتاجة تسجيل دخول بالشكل ده في الـ Header:

```
Authorization: Bearer {التوكن}
```

## الربط مع الفرونت إند (React)
- شكل بيانات الـ `CvDataDto` مطابق تمامًا لـ `CVData` الموجود في `src/pages/CVBuilder.tsx` بالفرونت، فمفيش تحويل معقد مطلوب.
- الفرونت شغال على `http://localhost:5173` أو `8080` والـ CORS مفعّل ليهم بالفعل في `Program.cs`. لو الـ Vite بيشتغل على بورت مختلف عندك، ضيفه في `FrontendPolicy` جوه `Program.cs`.
- الباك إند شغال افتراضيًا على `http://localhost:5080`.

## ملاحظة عن QuestPDF
المكتبة المستخدمة لتوليد الـ PDF (`QuestPDF`) شغالة بـ **Community License** المجاني، وده مناسب للأفراد والشركات الصغيرة (دخل سنوي أقل من مليون دولار). لو المشروع هيتحول لمنتج تجاري أكبر، راجع شروط الترخيص هنا: https://www.questpdf.com/license/
