import { Component, inject, OnInit } from '@angular/core';
import { StockService } from '../../core/services/stock.service';
import { Stock } from '../../shared/models/stock';
import { Pagination } from '../../shared/models/pagination';
import { CompanyParams } from '../../shared/models/companyParams';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzPaginationComponent } from 'ng-zorro-antd/pagination';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzDropDownDirective, NzDropdownMenuComponent } from 'ng-zorro-antd/dropdown';
import { NzMenuDirective, NzMenuItemComponent } from 'ng-zorro-antd/menu';
import { FiltersModalComponent } from './filters-modal/filters-modal.component';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { NzDividerComponent } from 'ng-zorro-antd/divider';

@Component({
  selector: 'app-companies',
  imports: [
    RouterLink,
    FormsModule,
    NzPaginationComponent,
    NzInputDirective,
    NzButtonComponent,
    NzDropDownDirective,
    NzDropdownMenuComponent,
    NzMenuDirective,
    NzMenuItemComponent,
    NzTableComponent,
    NzDividerComponent
  ],
  providers: [NzModalService],
  templateUrl: './companies.component.html',
  styleUrl: './companies.component.scss'
})
export class CompaniesComponent implements OnInit {
  stockService = inject(StockService);

  modalService = inject(NzModalService);
  companyParams = new CompanyParams();

  companies?: Pagination<Stock>;

  tickersPerPage = [10, 20, 30, 50]

  sortOptions = [
    { name: 'Default', value: '' },
    { name: 'Alphabetical (A-Z)', value: 'a-z' },
    { name: 'Alphabetical (Z-A)', value: 'z-a' }
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

  handlePageIndexChangeEvent(event: number) {
    this.companyParams.pageNumber = event;
    this.getCompanies();
  }

  handlePageSizeChangeEvent(event: number) {
    // https://github.com/NG-ZORRO/ng-zorro-antd/issues/5695
    this.companyParams.pageSize = event;
    this.companyParams.pageNumber = 1;
    this.getCompanies();
  }

  onSortChange(value: string) {
      this.companyParams.sort = value;
      this.companyParams.pageNumber = 1;
      this.getCompanies();
  }

  openFiltersDialog() {
    const modalRef = this.modalService.create({
      nzTitle: 'Filters',
      nzContent: FiltersModalComponent,
      nzWidth: '500px',
      nzData: {
        selectedIpoYears: this.companyParams.ipoYears,
        selectedCountries: this.companyParams.countries,
        selectedSectors: this.companyParams.sectors
      },
      nzFooter: null
    });

    modalRef.afterClose.subscribe({
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

  // https://www.reddit.com/r/Angular2/comments/1ihk7pb/why_not_use_protected_and_private_for_component/
  public resetParams() {
    this.companyParams.ipoYears = [];
    this.companyParams.countries = [];
    this.companyParams.sectors = [];
    this.companyParams.pageNumber = 1;
    this.companyParams.search = '';
    this.companyParams.sort = '';
    this.getCompanies();
  }
}
