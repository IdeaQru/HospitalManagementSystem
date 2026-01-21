# Hospital Management System 🏥  
Advanced OOP & Design Patterns with C# (.NET 9)

## 1. Deskripsi Proyek

Hospital Management System ini adalah aplikasi console berbasis C# dan .NET 9 yang dirancang untuk mendemonstrasikan penerapan **Object-Oriented Programming (OOP)**, **SOLID principles**, dan berbagai **design pattern** dalam arsitektur berlapis (layered / clean architecture).  

Fokus utama project ini adalah **struktur kode dan desain**, bukan UI atau koneksi database. Saat ini seluruh data disimpan menggunakan **in-memory repositories** (Dictionary).

---

## 2. Arsitektur & Layer

Proyek menggunakan pendekatan **Clean Architecture / Onion Architecture** dengan 4 layer utama:

- **Domain Layer**
  - Berisi core business: Entities, Value Objects, Enums, Interfaces Repository, dan Domain Exceptions.
  - Tidak bergantung pada layer lain.
- **Application Layer**
  - Berisi Application Services (use cases), Interfaces untuk design patterns, DTOs, dan Result objects.
  - Mengorkestrasi Domain + Infrastructure.
- **Infrastructure Layer**
  - Berisi implementasi konkret: Repositories (in-memory), Factories, Strategies, Observers, dan Configuration (Singleton).
- **Presentation Layer**
  - Berisi `Program.cs` untuk demo console end-to-end.

Struktur folder utama:

```text
src/
  HospitalManagementSystem.Domain/
  HospitalManagementSystem.Application/
  HospitalManagementSystem.Infrastructure/
  HospitalManagementSystem.Presentation/

link:https://github.com/ideaqru