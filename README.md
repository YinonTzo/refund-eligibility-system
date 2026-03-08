# Refund Eligibility System

מערכת Full-stack לניהול וחישוב זכאות להחזר כספי לאזרחים, הכוללת ממשק פקיד (Back-office) וממשק אזרח.

## הוראות הרצה

ניתן להריץ את כל הסביבה (Database, Backend, UI) באמצעות פקודה אחת מתיקיית השורש:
docker compose up

### קישורים לגישה:
* UI (React): http://localhost:3000
* Backend API Documentation (Scalar): https://localhost:8080/scalar/v1

---

## גישה למסד הנתונים (SQL Server)

ניתן להתחבר ל-DB לצורך תשאול נתונים באמצעות כלי ניהול (SSMS):

* Server: localhost,1433
* User: sa
* Password: passw0rd!

---

## ארכיטקטורה ודגשים טכניים

### Backend (.NET Core)
* Clean Architecture: המערכת מחולקת לשכבות (Domain, Application, Infrastructure) להפרדה מלאה בין הלוגיקה העסקית למימוש.

### Database & Concurrency
אבטחת המידע ושלמות הנתונים מבוצעת ברמת ה-Database:
* Race Condition Prevention: שימוש ב-Stored Procedures אטומיים.
* Explicit Locking: שימוש ב-UPDLOCK ו-ROWLOCK בתהליך אישור הבקשה ועדכון התקציב למניעת אישור כפול או משיכת יתר.

### Frontend (React + Tailwind 4)

---

## מבנה הפרויקט
* RefundEligibilitySystem/ - Backend.
* refund-client/ - Frontend.
* Infrastructure/Scripts/ - סקריפטים של SQL ליצירת ה-Schema, הפרוצדורות ונתוני הבסיס.
