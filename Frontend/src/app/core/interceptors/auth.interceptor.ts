import { HttpInterceptorFn } from '@angular/common/http';

// automate sending cookie to server with every request

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    withCredentials: true
  })
  return next(clonedRequest);
};
