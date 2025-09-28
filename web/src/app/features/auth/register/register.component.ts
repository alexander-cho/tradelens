import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
// import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,
    // MatInput,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private formBuilder = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  validationErrors?: string[];

  registerForm = this.formBuilder.group({
    firstName: [''],
    lastName: [''],
    email: [''],
    password: ['']
  });

  onSubmit() {
    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/login');
      },
      error: errors => {
        this.validationErrors = Object.values(errors.error.errors).flat() as string[];
      }
    });
  }
}
