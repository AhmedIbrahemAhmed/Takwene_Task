import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TrackDetail as TrackDetailModel } from '../models/track.model';
import { TrackService } from '../services/trackService';

@Component({
  selector: 'app-track-detail',
  imports: [CommonModule, RouterModule],
  templateUrl: './track-detail.html',
  styleUrl: './track-detail.css',
})
export class TrackDetail implements OnInit {
  private trackService = inject(TrackService);
  private route = inject(ActivatedRoute);

  track = signal<TrackDetailModel | null>(null);
  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    const id = idParam ? Number(idParam) : 0;
    if (!id) {
      this.error.set('Invalid track id');
      return;
    }
    this.loadTrack(id);
  }

  loadTrack(id: number): void {
    this.loading.set(true);
    this.error.set('');
    this.trackService.getTrackById(id).subscribe({
      next: (data) => { this.track.set(data); this.loading.set(false); },
      error: () => { this.error.set('Failed to load track.'); this.loading.set(false); }
    });
  }

  back(): void {
    history.back();
  }
}
