import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // req is immutable by default, clone to modify it, pass credentials with each request
  const clonedRequest = req.clone({
    withCredentials: true
  });

  return next(clonedRequest);
};

