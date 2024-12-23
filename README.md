# Flight Search System API Documentation

## 1. Genel Bak��
Flight Search System, u�u� arama ve rezervasyon i�lemlerini y�neten bir mikroservis mimarisidir. Sistem, �e�itli havayolu sa�lay�c�lar�ndan (HopeAir ve AybJet) u�u� bilgilerini toplar ve merkezi bir API �zerinden sunar.

## 2. Base URL
```
http://localhost:5000
```

## 3. Endpoints

### 3.1. Flight Search API

#### Search Flights
**Endpoint:**
```
POST /api/flight/search
```
**Description:**
Belirtilen kriterlere g�re mevcut u�u�lar� arar.

**Request Body:**
```json
{
    "origin": "IST",          // Kalk�� havaalan� kodu (required)
    "destination": "LHR",     // Var�� havaalan� kodu (required)
    "departureDate": "2024-11-14T00:00:00Z",  // Kalk�� tarihi (required)
    "returnDate": null,       // D�n�� tarihi (optional)
    "passengerCount": 1       // Yolcu say�s� (required)
}
```
**Response (200 OK):**
```json
[
    {
        "flightNumber": "HH123",
        "departure": "IST",
        "arrival": "LHR",
        "price": 350.50,
        "currency": "USD",
        "duration": "4h 20m",
        "departureTime": "2024-11-14T10:00:00",
        "arrivalTime": "2024-11-14T14:20:00",
        "providerName": "HopeAir"
    }
]
```

### 3.2. Booking API

#### Create Booking
**Endpoint:**
```
POST /api/booking/create
```
**Description:**
Yeni bir u�u� rezervasyonu olu�turur.

**Request Body:**
```json
{
    "flightNumber": "HH123",  // U�u� numaras� (required)
    "passengers": [           // Yolcu listesi (required)
        {
            "firstName": "John",           // �sim (required)
            "lastName": "Doe",             // Soyisim (required)
            "dateOfBirth": "1990-01-01",   // Do�um tarihi (required)
            "passportNumber": "AB123456"   // Pasaport numaras� (optional)
        }
    ]
}
```

#### Get Booking Details
**Endpoint:**
```
GET /api/booking/{id}
```
**Description:**
Rezervasyon detaylar�n� getirir.

**Parameters:**
- `id` (UUID, required): Rezervasyon ID'si

**Response (200 OK):**
```json
{
    "bookingId": "guid",
    "flightNumber": "HH123",
    "status": "Confirmed",
    "passengers": [
        {
            "firstName": "John",
            "lastName": "Doe",
            "dateOfBirth": "1990-01-01",
            "passportNumber": "AB123456"
        }
    ]
}
```

#### Cancel Booking
**Endpoint:**
```
PUT /api/booking/{id}/cancel
```
**Description:**
Rezervasyonu iptal eder.

**Parameters:**
- `id` (UUID, required): �ptal edilecek rezervasyon ID'si

### 3.3. Provider APIs

#### HopeAir Flights
**Endpoint:**
```
GET /api/hopeair/flights
```
**Description:**
HopeAir sa�lay�c�s�ndan u�u� bilgilerini getirir.

**Request Body:**
```json
{
    "origin": "IST",
    "destination": "LHR",
    "departureDate": "2024-11-14T00:00:00Z"
}
```

#### AybJet Flights
**Endpoint:**
```
GET /api/aybjet/flights
```
**Description:**
AybJet sa�lay�c�s�ndan u�u� bilgilerini getirir.

**Request Body:**
```json
{
    "origin": "IST",
    "destination": "LHR",
    "departureDate": "2024-11-14T00:00:00Z"
}
```

## 4. Veri Modelleri

### 4.1. FlightInfo
```csharp
public record FlightInfo
{
    public string FlightNumber { get; init; }    // U�u� numaras�
    public string Departure { get; init; }       // Kalk�� havaalan� kodu
    public string Arrival { get; init; }         // Var�� havaalan� kodu
    public decimal Price { get; init; }          // Bilet fiyat�
    public string Currency { get; init; }        // Para birimi
    public string Duration { get; init; }        // U�u� s�resi
    public DateTime DepartureTime { get; init; } // Kalk�� zaman�
    public DateTime ArrivalTime { get; init; }   // Var�� zaman�
    public string ProviderName { get; init; }    // Sa�lay�c� ad�
}
```

### 4.2. BookingRequest
```csharp
public record BookingRequest
{
    public string FlightNumber { get; init; }
    public List<PassengerInfo> Passengers { get; init; }
}

public record PassengerInfo
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string? PassportNumber { get; init; }
}
```

## 5. Hata Kodlar�

- **200 OK:** ��lem ba�ar�l�
- **400 Bad Request:** Ge�ersiz istek format� veya eksik/hatal� parametre
- **404 Not Found:** �stenilen kaynak bulunamad�
- **500 Internal Server Error:** Sunucu hatas�

## 6. Rate Limiting

- Her IP adresi i�in dakikada maksimum 100 istek
- Booking i�lemleri i�in dakikada maksimum 10 istek

## 7. Security

- T�m istekler HTTPS �zerinden yap�lmal�d�r
- API Gateway �zerinden y�nlendirme yap�l�r
- Her servis i�in health check endpoints mevcuttur

## 8. Notlar

- **Tarih format�:** ISO 8601 (YYYY-MM-DDTHH:mm:ssZ)
- **Para birimi:** Varsay�lan olarak USD
- **Havaalan� kodlar�:** IATA format�nda olmal�d�r
