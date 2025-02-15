import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { StockService } from '../../core/services/stock.service';
import { Stock } from '../../shared/models/stock';
import { Pagination } from '../../shared/models/pagination';
import { CompanyParams } from '../../shared/models/companyParams';
import { RouterLink } from '@angular/router';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatMenu, MatMenuModule } from '@angular/material/menu';
import { FormsModule } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-companies',
  imports: [
    NavbarComponent,
    RouterLink,
    MatListOption,
    MatMenu,
    MatSelectionList,
    FormsModule,
    MatPaginator,
    MatMenuModule
  ],
  templateUrl: './companies.component.html',
  styleUrl: './companies.component.scss'
})
export class CompaniesComponent implements OnInit{
  stockService = inject(StockService);
  dialogService = inject(MatDialog);
  companyParams = new CompanyParams();

  companies?: Pagination<Stock>;

  tickersPerPage = [10, 20, 30, 50]

  sortOptions = [
    {name: 'Default', value: ''},
    {name: 'Alphabetical (A-Z)', value: 'a-z'},
    {name: 'Alphabetical (Z-A)', value: 'z-a'}
  ]

  getCompanies() {
    this.stockService.getStocks(this.companyParams).subscribe({
      next: response => this.companies = response,
      error: error => console.log(error)
    });
  }

  ngOnInit() {
    this.stockService.getIpoYears();
    this.stockService.getCountries();
    this.stockService.getSectors();
    this.getCompanies();
  }

  onSearchChange() {
    this.companyParams.pageNumber = 1;
    this.getCompanies();
  }

  handlePageEvent(event: PageEvent) {
    this.companyParams.pageSize = event.pageSize;
    this.companyParams.pageNumber = event.pageIndex + 1;
    this.getCompanies();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.companyParams.sort = selectedOption.value;
      this.companyParams.pageNumber = 1;
      this.getCompanies();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '900px',
      maxHeight: '700px',
      data: {
        selectedIpoYears: this.companyParams.ipoYears,
        selectedCountries: this.companyParams.countries,
        selectedSectors: this.companyParams.sectors
      }
    });

    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.companyParams.ipoYears = result.selectedIpoYears;
          this.companyParams.countries = result.selectedCountries;
          this.companyParams.sectors = result.selectedSectors;
          this.companyParams.pageNumber = 1;
          this.getCompanies();
        }
      }
    });
  }

  resetParams() {
    this.companyParams.ipoYears = [];
    this.companyParams.countries = [];
    this.companyParams.sectors = [];
    this.companyParams.pageNumber = 1;
    this.companyParams.search = '';
    this.companyParams.sort = '';
    this.getCompanies();
  }
}
