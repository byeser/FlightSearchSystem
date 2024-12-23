# Flight Search System API Documentation

## 1. Genel Bakýþ
Flight Search System, uçuþ arama ve rezervasyon iþlemlerini yöneten bir mikroservis mimarisidir. Sistem, çeþitli havayolu saðlayýcýlarýndan (HopeAir ve AybJet) uçuþ bilgilerini toplar ve merkezi bir API üzerinden sunar.

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
Belirtilen kriterlere göre mevcut uçuþlarý arar.

**Request Body:**
```json
{
    "origin": "IST",          // Kalkýþ havaalaný kodu (required)
    "destination": "LHR",     // Varýþ havaalaný kodu (required)
    "departureDate": "2024-11-14T00:00:00Z",  // Kalkýþ tarihi (required)
    "returnDate": null,       // Dönüþ tarihi (optional)
    "passengerCount": 1       // Yolcu sayýsý (required)
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
Yeni bir uçuþ rezervasyonu oluþturur.

**Request Body:**
```json
{
    "flightNumber": "HH123",  // Uçuþ numarasý (required)
    "passengers": [           // Yolcu listesi (required)
        {
            "firstName": "John",           // Ýsim (required)
            "lastName": "Doe",             // Soyisim (required)
            "dateOfBirth": "1990-01-01",   // Doðum tarihi (required)
            "passportNumber": "AB123456"   // Pasaport numarasý (optional)
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
Rezervasyon detaylarýný getirir.

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
- `id` (UUID, required): Ýptal edilecek rezervasyon ID'si

### 3.3. Provider APIs

#### HopeAir Flights
**Endpoint:**
```
GET /api/hopeair/flights
```
**Description:**
HopeAir saðlayýcýsýndan uçuþ bilgilerini getirir.

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
AybJet saðlayýcýsýndan uçuþ bilgilerini getirir.

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
    public string FlightNumber { get; init; }    // Uçuþ numarasý
    public string Departure { get; init; }       // Kalkýþ havaalaný kodu
    public string Arrival { get; init; }         // Varýþ havaalaný kodu
    public decimal Price { get; init; }          // Bilet fiyatý
    public string Currency { get; init; }        // Para birimi
    public string Duration { get; init; }        // Uçuþ süresi
    public DateTime DepartureTime { get; init; } // Kalkýþ zamaný
    public DateTime ArrivalTime { get; init; }   // Varýþ zamaný
    public string ProviderName { get; init; }    // Saðlayýcý adý
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

## 5. Hata Kodlarý

- **200 OK:** Ýþlem baþarýlý
- **400 Bad Request:** Geçersiz istek formatý veya eksik/hatalý parametre
- **404 Not Found:** Ýstenilen kaynak bulunamadý
- **500 Internal Server Error:** Sunucu hatasý

## 6. Rate Limiting

- Her IP adresi için dakikada maksimum 100 istek
- Booking iþlemleri için dakikada maksimum 10 istek

## 7. Security

- Tüm istekler HTTPS üzerinden yapýlmalýdýr
- API Gateway üzerinden yönlendirme yapýlýr
- Her servis için health check endpoints mevcuttur

## 8. Notlar

- **Tarih formatý:** ISO 8601 (YYYY-MM-DDTHH:mm:ssZ)
- **Para birimi:** Varsayýlan olarak USD
- **Havaalaný kodlarý:** IATA formatýnda olmalýdýr
