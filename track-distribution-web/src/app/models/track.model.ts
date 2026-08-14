export interface Track {
  id: number;
  title: string;
  artistId: number;
  artistName: string;
  isrc: string;
  releaseDate: string;
  genre: string;
  status: string;
}

export interface TrackDistribution {
  dspId: number;
  dspName: string;
  status: string;
  submittedAt: string;
}

export interface TrackDetail extends Track {
  distributions: TrackDistribution[];
}