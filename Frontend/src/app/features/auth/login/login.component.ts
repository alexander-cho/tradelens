import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, MatCard, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private formBuilder = inject(FormBuilder);
  private authService = inject(AuthService);

  private router = inject(Router);

  loginForm = this.formBuilder.group({
    email: [''],
    password: ['']
  });

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.authService.getUserInfo().subscribe();
        this.router.navigateByUrl('/');
      }
    });
  }
}
