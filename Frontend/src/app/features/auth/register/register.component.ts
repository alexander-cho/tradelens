import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ButtonModule } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CardModule, IconFieldModule, InputIconModule, ButtonModule, InputText],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  registerForm = this.fb.group({
    firstName: [''],
    lastName: [''],
    email: [''],
    password: ['']
  });

  onSubmit() {
    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/auth/login');
      }
    });
  }
}
