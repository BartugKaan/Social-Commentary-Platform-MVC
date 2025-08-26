# MVC Project Camp - Social commentary platform

A comprehensive Social commentary platform  built with ASP.NET MVC following N-Tier Architecture principles.


## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [Authentication & Authorization](#authentication--authorization)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

MVC Project Camp is a multi-role content management system that serves as social commentary platform. The application supports multiple user roles including administrators, writers, and public visitors, each with distinct functionalities and access levels.

# 🖼️🖼 Project Screenshots
<img width="2558" height="1262" alt="Ekran görüntüsü 2025-08-26 145600" src="https://github.com/user-attachments/assets/f236fcff-03ea-48b3-b2c6-e76f0782fff8" />
<img width="2559" height="1281" alt="Ekran görüntüsü 2025-08-26 144327" src="https://github.com/user-attachments/assets/e4235e36-eb5c-417c-93d5-0686ba50412d" />
<img width="2553" height="1279" alt="Ekran görüntüsü 2025-08-26 144309" src="https://github.com/user-attachments/assets/7f187693-0b01-4e67-9224-87ebe6e7f860" />
<img width="2556" height="1274" alt="Ekran görüntüsü 2025-08-26 144257" src="https://github.com/user-attachments/assets/a16eda5d-b1ec-4e6f-b4f8-2cb6bb4c15b8" />
<img width="2559" height="1272" alt="Ekran görüntüsü 2025-08-26 144239" src="https://github.com/user-attachments/assets/9c0b92b6-999f-4cb2-b584-0fb446080c86" />
<img width="2558" height="1277" alt="Ekran görüntüsü 2025-08-26 144157" src="https://github.com/user-attachments/assets/fb962637-acee-4b81-ac69-49b70a10df29" />

## ✨ Features

### 🔐 Authentication & Authorization
- **Admin Panel**: Full system administration with category, writer, and content management
- **Writer Panel**: Content creation, heading management, and messaging system
- **Public Showcase**: Article browsing, reading, and responsive design
- **Secure Login**: BCrypt password hashing and Forms Authentication

### 📝 Content Management
- **Dynamic Categories**: Hierarchical category management
- **Article System**: Rich content creation with HTML support
- **Heading Management**: Organize articles under specific headings
- **Status Control**: Active/inactive content management

### 💬 Communication
- **Messaging System**: Internal messaging between writers
- **Contact Forms**: Public contact functionality
- **Real-time Notifications**: Success/error message handling

### 🎨 User Interface
- **Responsive Design**: Bootstrap-based responsive layouts
- **AdminLTE Integration**: Professional admin dashboard
- **Modern UI Components**: Font Awesome icons, animated elements
- **Print Functionality**: Article printing capabilities

### 🔍 Advanced Features
- **Reading Progress**: Article reading progress indicator
- **Social Sharing**: Native and fallback sharing options
- **Content Statistics**: Word count, character count analytics
- **Author Profiles**: Writer information and avatar system

## 🏗️ Architecture

The project follows **N-Tier Architecture** with clear separation of concerns:

```
┌─────────────────┐
│  Presentation   │  ← MvcProjeKampi (Controllers, Views, Models)
│     Layer       │
├─────────────────┤
│   Business      │  ← BusinessLayer (Business Logic, Validation)
│     Layer       │
├─────────────────┤
│  Data Access    │  ← DataAccessLayer (Repository Pattern, EF)
│     Layer       │
├─────────────────┤
│   Entity        │  ← EntityLayer (Domain Models, POCOs)
│     Layer       │
└─────────────────┘
```

### Design Patterns Used
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loose coupling between layers
- **MVC Pattern**: Model-View-Controller separation
- **Unit of Work**: Transaction management
- **Factory Pattern**: Object creation management

## 🛠️ Technologies Used

### Backend
- **Framework**: ASP.NET MVC 5.2.9 (.NET Framework 4.6.2)
- **ORM**: Entity Framework 6.5.1 (Code First)
- **Database**: SQL Server
- **Authentication**: Forms Authentication + BCrypt
- **Validation**: FluentValidation 10.0.4

### Frontend
- **CSS Framework**: Bootstrap 5.2.3
- **Icons**: Font Awesome
- **JavaScript**: jQuery 3.7.0, jQuery Validation
- **UI Components**: AdminLTE 3.0.4
- **Styling**: Custom CSS with animations

### Development Tools
- **IDE**: Visual Studio 2019/2022
- **Version Control**: Git
- **Package Management**: NuGet

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- SQL Server 2016 or later
- .NET Framework 4.6.2 or later
- IIS Express (included with Visual Studio)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/BartugKaan/MvcProjectCamp.git
   cd MvcProjectCamp
   ```

2. **Restore NuGet packages**
   ```bash
   nuget restore
   ```

3. **Update database connection string**
   ```xml
   <!-- In Web.config -->
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Server=.;Database=MvcProjeKampiDb;Trusted_Connection=true;" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **Enable Code First Migrations**
   ```bash
   Enable-Migrations -ProjectName DataAccessLayer
   Add-Migration InitialCreate -ProjectName DataAccessLayer
   Update-Database -ProjectName DataAccessLayer
   ```

5. **Create test admin account**
   ```
   Navigate to: /Login/CreateTestAdmin
   Username: admin
   Password: 123456
   ```

6. **Run the application**
   ```
   Press F5 in Visual Studio or
   dotnet run --project MvcProjeKampi
   ```

## 📁 Project Structure

```
MvcProjeKampi/
├── 📂 EntityLayer/                 # Domain Models
│   ├── Concrete/                   # Entity classes
│   └── Abstract/                   # Base interfaces
├── 📂 DataAccessLayer/             # Data Access
│   ├── Abstract/                   # Repository interfaces
│   ├── Concrete/                   # Repository implementations
│   └── EntityFramework/            # EF Context & configurations
├── 📂 BusinessLayer/               # Business Logic
│   ├── Abstract/                   # Service interfaces
│   ├── Concrete/                   # Service implementations
│   └── ValidationRules/            # FluentValidation rules
├── 📂 MvcProjeKampi/              # Web Application
│   ├── Controllers/                # MVC Controllers
│   ├── Views/                      # Razor Views
│   ├── Models/                     # ViewModels
│   ├── Filters/                    # Custom Filters
│   └── Content/                    # Static files (CSS, JS, Images)
└── 📂 AdminLTE-3.0.4/             # UI Framework
```

## 🗄️ Database Schema

### Core Tables
- **Categories**: Content categorization
- **Writers**: Author management
- **Headings**: Article organization
- **Contents**: Main content storage
- **Admins**: System administrators
- **Messages**: Internal messaging
- **Abouts**: About page content
- **Contacts**: Contact form submissions

### Key Relationships
```sql
Categories (1) ←→ (N) Headings
Writers (1) ←→ (N) Headings
Writers (1) ←→ (N) Contents
Headings (1) ←→ (N) Contents
```

## 🔐 Authentication & Authorization

### Role-Based Access Control

| Role | Access Level | Capabilities |
|------|-------------|--------------|
| **Public** | Read-only | Browse articles, read content |
| **Writer** | Content Creator | Create/edit own content, messaging |
| **Admin** | Full Access | Manage all content, users, system settings |

### Security Features
- **Password Hashing**: BCrypt with salt
- **Session Management**: Secure session handling
- **Authorization Filters**: Role-based action filtering
- **CSRF Protection**: Form token validation
- **Input Validation**: XSS prevention

## 📡 Key Endpoints

### Public Routes
- `GET /` - Home/showcase page
- `GET /Vitrin/HeadingContents/{id}` - Articles by heading
- `GET /Vitrin/ArticleDetail/{id}` - Article details

### Admin Routes
- `GET /Login` - Admin login
- `GET /AdminCategory` - Category management
- `GET /AdminWriter` - Writer management
- `GET /AdminHeading` - Heading management

### Writer Routes
- `GET /WriterLogin` - Writer login
- `GET /WriterPanel/WriterProfile` - Writer dashboard
- `GET /WriterPanel/MyContents` - Content management

## 🧪 Testing

### Manual Testing
1. **Admin Flow**: Login → Manage categories → Add writers → Monitor content
2. **Writer Flow**: Register → Login → Create headings → Write articles → Send messages
3. **Public Flow**: Browse → Read articles → Contact → Share content

### Test Accounts
```
Admin: admin / 123456
Writer: Create via admin panel or registration
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request


## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Bartuğ Kaan**
- GitHub: [@BartugKaan](https://github.com/BartugKaan)
- Project Link: [MvcProjectCamp](https://github.com/BartugKaan/MvcProjectCamp)

## 🙏 Acknowledgments

- **AdminLTE**: For the beautiful admin dashboard template
- **Bootstrap**: For responsive design framework
- **Entity Framework**: For powerful ORM capabilities
- **FluentValidation**: For elegant validation rules
- **BCrypt.NET**: For secure password hashing
- **Murat Yücedağ**: For amazing tutorials and guidance on ASP.NET MVC

---

**⭐ If you found this project helpful, please consider giving it a star!**

## 📞 Support

If you have any questions or need help with setup, please open an issue on GitHub.

---
