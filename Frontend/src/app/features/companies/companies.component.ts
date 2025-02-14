import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { StockService } from '../../core/services/stock.service';
import { Stock } from '../../shared/models/stock';

@Component({
  selector: 'app-companies',
  imports: [
    NavbarComponent,
  ],
  templateUrl: './companies.component.html',
  styleUrl: './companies.component.scss'
})
export class CompaniesComponent implements OnInit{
  stockService = inject(StockService);

  companies?: Stock[];

  getCompanies() {
    this.stockService.getStocks().subscribe({
      next: response => {
        this.companies = response;
        console.log('Companies Length:', this.companies?.length || 'No companies');
      },
      error: error => console.log(error)
    });
  }

  ngOnInit() {
    this.getCompanies();
  }
}
