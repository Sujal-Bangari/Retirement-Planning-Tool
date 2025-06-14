import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl='http://localhost:5010/api/validate/login';
  constructor(private http:HttpClient) { }
  login(email: string, password: string)
  {
    return this.http.post(this.apiUrl, {email, password});
  }
}
