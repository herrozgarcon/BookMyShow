export interface Movie { id:number; title:string; description:string; genre:string; releaseDate:string; duration:number; language:string }
export interface Theatre { id:number; name:string; location:string }
export interface Screen { screensId:number; screenName:string; theatreId:number }
export interface Seat { seatId:number; screensId:number; rowName:string; seatNumber:number; seatType:string }
export interface Show { showId:number; movieId:number; screensId:number; startTime:string; endTime:string; ticketPrice:number; theatreName?:string; movieTitle?:string }
export interface ShowSeat { showSeatId:number; seatId:number; rowName:string; seatNumber:number; seatType:string; status:string }
export interface Booking { bookingId:number; bookingReference:string; showId:number; customerName:string; bookingDate:string; totalAmount:number; status:string; seats:{seatId:number;price:number}[] }
export interface AuthResponse { userId:number;name:string;email:string;role:string;accessToken:string;expiresAtUtc:string }
export interface Profile { appUserId:number;name:string;email:string;role:string }
