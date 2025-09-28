import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzColDirective, NzRowDirective } from 'ng-zorro-antd/grid';
import { NzFormControlComponent, NzFormDirective, NzFormItemComponent } from 'ng-zorro-antd/form';
import { NzInputDirective, NzInputGroupComponent } from 'ng-zorro-antd/input';
// import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,
    // MatInput,
    RouterLink, NzButtonComponent, NzColDirective, NzFormControlComponent, NzFormDirective, NzFormItemComponent, NzInputDirective, NzInputGroupComponent, NzRowDirective
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
