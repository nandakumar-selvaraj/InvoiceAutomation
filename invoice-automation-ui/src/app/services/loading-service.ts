import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
    providedIn: 'root',
})
export class LoadingService {
    private loadingCount = 0;
    private isLoadingSubject = new BehaviorSubject<boolean>(false);

    // Observable for components to subscribe to
    isLoading$ = this.isLoadingSubject.asObservable();

    show() {
        this.loadingCount++;
        this.isLoadingSubject.next(true);
    }

    hide() {
        this.loadingCount--;
        if (this.loadingCount <= 0) {
            this.loadingCount = 0;
            this.isLoadingSubject.next(false);
        }
    }
}
