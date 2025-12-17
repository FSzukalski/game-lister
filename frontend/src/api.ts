// src/api.ts

// Adres backendu – na razie na sztywno
export const API_BASE_URL = "http://localhost:5154";

export interface MoneyDto {
  amount: number;
  currency: string;
}

export interface VideoAnalysisResultDto {
  rawTitle: string;
  detectedTitle?: string | null;
  platform: string;
  edition?: string | null;
  language?: string | null;
  region?: string | null;
  condition: string;
  isBoxIncluded: boolean;
  isManualIncluded: boolean;
  isOriginal: boolean;
  suggestedPrice?: MoneyDto | null;
  spokenDescription?: string | null;
  screenshotUrls: string[];
}

export interface VideoIntakeResultDto {
  gameId: number;
  listingDraftId: number;
}

export interface ListingPreviewDto {
  listingDraftId: number;
  gameId: number;
  marketplace: string;
  title: string;
  subtitle?: string | null;
  descriptionHtml: string;
  price: MoneyDto;
  generatedAtUtc: string;
}

export interface AllegroOfferDraftPayloadDto {
  title: string;
  descriptionHtml: string;
  categoryId: string;
  language: string;
  price: MoneyDto;
  imageUrls: string[];
}

// ---- Proste funkcje do rozmowy z backendem ----

export async function mockCreateFromVideo(
  payload: VideoAnalysisResultDto
): Promise<VideoIntakeResultDto> {
  const res = await fetch(`${API_BASE_URL}/api/intake/mock-from-video`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`mock-from-video failed: ${res.status} ${text}`);
  }

  return res.json();
}

export async function getListingPreview(
  listingDraftId: number
): Promise<ListingPreviewDto> {
  const res = await fetch(
    `${API_BASE_URL}/api/ListingDrafts/${listingDraftId}/preview`
  );

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`get preview failed: ${res.status} ${text}`);
  }

  return res.json();
}

export async function getAllegroPayload(
  listingDraftId: number
): Promise<AllegroOfferDraftPayloadDto> {
  const res = await fetch(
    `${API_BASE_URL}/api/ListingDrafts/${listingDraftId}/allegro-payload`
  );

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`get allegro payload failed: ${res.status} ${text}`);
  }

  return res.json();
}
