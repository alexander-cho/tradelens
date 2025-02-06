import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputText } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CardModule, IconFieldModule, InputIconModule, ButtonModule, InputText, CheckboxModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  // redirect user after logging in
  private router = inject(Router);

  loginForm = this.fb.group({
    email: [''],
    password: ['']
  });

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.authService.getUserInfo().subscribe();
        this.router.navigateByUrl('');
      }
    })
  }
}
