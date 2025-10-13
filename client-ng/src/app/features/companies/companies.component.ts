import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { StockService } from '../../core/services/stock.service';
import { Stock } from '../../shared/models/stock';
import { Pagination } from '../../shared/models/pagination';
import { CompanyParams } from '../../shared/models/company-params';
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
import { toObservable } from '@angular/core/rxjs-interop';
import { debounceTime, Observable } from 'rxjs';

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
    NzTableComponent
  ],
  providers: [NzModalService],
  templateUrl: './companies.component.html',
  styleUrl: './companies.component.scss'
})
export class CompaniesComponent implements OnInit {
  stockService = inject(StockService);
  modalService = inject(NzModalService);

  searchTerm: WritableSignal<string> = signal<string>("");
  debouncedSearch: Observable<string> = toObservable(this.searchTerm).pipe(
    debounceTime(1000)
  );

  companies: WritableSignal<Pagination<Stock> | undefined> = signal(undefined);

  companyParams = new CompanyParams();
  tickersPerPage = [10, 20, 30, 50];
  sortOptions = [
    { name: 'Default', value: '' },
    { name: 'Alphabetical (A-Z)', value: 'a-z' },
    { name: 'Alphabetical (Z-A)', value: 'z-a' }
  ];

  ngOnInit() {
    this.stockService.getIpoYears();
    this.stockService.getCountries();
    this.stockService.getSectors();
    this.getCompanies();

    this.debouncedSearch.subscribe(() => {
      this.companyParams.pageNumber = 1;
      this.companyParams.search = this.searchTerm();
      this.getCompanies();
    });
  }

  // // dynamic search using effects
  // searchChange = effect(() => {
  //   this.companyParams.pageNumber = 1;
  //   this.companyParams.search = this.searchTerm();
  //   this.getCompanies();
  // });

  getCompanies() {
    this.stockService.getStocks(this.companyParams).subscribe({
      next: response => this.companies?.set(response),
      error: error => console.log(error)
    });
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

  openFiltersModal() {
    const modalRef = this.modalService.create({
      nzTitle: 'Filters',
      nzContent: FiltersModalComponent,
      nzWidth: '600px',
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
