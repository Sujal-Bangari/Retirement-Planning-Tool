import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
})
export class LoginComponent {
  email: string='';
  password: string='';
  loginSuccess: string='';
  isLoggedIn: boolean = false; // ✅ Tracks login state dynamically
  constructor(private authService: AuthService, private router: Router){}
  onSubmit(){
    this.authService.login(this.email,this.password).subscribe(
      (response: any) => {
        if (response) {
          this.loginSuccess = 'Login Successful!';
          this.isLoggedIn=true;
          console.log(response)
          // if (typeof window !== 'undefined') {
          //   sessionStorage.setItem('userData', JSON.stringify(response));
          // }
          // //localStorage.setItem('userData', JSON.stringify(response));
          // this.router.navigate(['/main']);
          
          this.router.navigate(['/main'], {
            queryParams: {
              firstName: response.firstName,
              lastName: response.lastName,
              email: response.email,
              gender: response.gender,
              age: response.age,
              pId: response.pId
            }
    
          });
        } 
        // else {
        //   this.loginSuccess = 'Invalid credentials. Please try again.';
        // }
      },
      (error) => {
        this.loginSuccess = 'Login failed. Please check your details.';
      }
    );
  }
}

