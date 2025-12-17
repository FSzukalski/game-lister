import { useState } from "react";
import type {
  AllegroOfferDraftPayloadDto,
  ListingPreviewDto,
  MoneyDto,
  VideoAnalysisResultDto,
  VideoIntakeResultDto,
} from "./api";

// funkcje – normalny import runtime
import {
  mockCreateFromVideo,
  getListingPreview,
  getAllegroPayload,
} from "./api";

type LoadingState = "idle" | "submitting" | "loadingPreview" | "loadingPayload";

function App() {
  const [rawTitle, setRawTitle] = useState(
    "Mario Kart 8 na Switcha, stan bardzo dobry"
  );
  const [platform, setPlatform] = useState("Nintendo Switch");
  const [condition, setCondition] = useState("Very good");
  const [amount, setAmount] = useState("129.99");

  const [lastResult, setLastResult] = useState<VideoIntakeResultDto | null>(
    null
  );
  const [preview, setPreview] = useState<ListingPreviewDto | null>(null);
  const [allegroPayload, setAllegroPayload] =
    useState<AllegroOfferDraftPayloadDto | null>(null);

  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<LoadingState>("idle");

  const handleGenerateClick = async () => {
    setError(null);
    setPreview(null);
    setAllegroPayload(null);
    setLastResult(null);

    let price: number | null = null;
    if (amount.trim() !== "") {
      const parsed = Number(amount.replace(",", "."));
      if (!Number.isNaN(parsed)) {
        price = parsed;
      }
    }

    const suggestedPrice: MoneyDto | null =
      price === null
        ? null
        : {
            amount: price,
            currency: "PLN",
          };

    const payload: VideoAnalysisResultDto = {
      rawTitle,
      detectedTitle: "Mario Kart 8 Deluxe",
      platform,
      edition: "Deluxe",
      language: "PL/EN",
      region: "PAL",
      condition,
      isBoxIncluded: true,
      isManualIncluded: true,
      isOriginal: true,
      suggestedPrice,
      spokenDescription:
        "Gra w bardzo dobrym stanie, komplet pudełko + instrukcja, działa bez zarzutu.",
      screenshotUrls: [
        "https://example.com/mario-front.jpg",
        "https://example.com/mario-back.jpg",
      ],
    };

    try {
      setLoading("submitting");
      const result = await mockCreateFromVideo(payload);
      setLastResult(result);

      // od razu dociągamy preview i payload
      setLoading("loadingPreview");
      const previewData = await getListingPreview(result.listingDraftId);
      setPreview(previewData);

      setLoading("loadingPayload");
      const payloadData = await getAllegroPayload(result.listingDraftId);
      setAllegroPayload(payloadData);

      setLoading("idle");
    } catch (e: any) {
      console.error(e);
      setError(e.message ?? "Unknown error");
      setLoading("idle");
    }
  };

  const isBusy = loading !== "idle";

  return (
    <div className="app-root">
      <header className="app-header">
        <h1>Game Lister – video → auction draft</h1>
        <p>
          Mock: symulujemy wynik analizy wideo i generujemy szkic aukcji +
          payload Allegro.
        </p>
      </header>

      <main className="app-main">
        <section className="card">
          <h2>1. Mock „video analysis” input</h2>
          <div className="form-grid">
            <label>
              <span>Raw title (z wideo / z mowy)</span>
              <input
                type="text"
                value={rawTitle}
                onChange={(e) => setRawTitle(e.target.value)}
              />
            </label>

            <label>
              <span>Platforma</span>
              <input
                type="text"
                value={platform}
                onChange={(e) => setPlatform(e.target.value)}
              />
            </label>

            <label>
              <span>Stan</span>
              <input
                type="text"
                value={condition}
                onChange={(e) => setCondition(e.target.value)}
              />
            </label>

            <label>
              <span>Sugerowana cena (PLN)</span>
              <input
                type="text"
                value={amount}
                onChange={(e) => setAmount(e.target.value)}
              />
            </label>
          </div>

          <button
            className="primary-btn"
            onClick={handleGenerateClick}
            disabled={isBusy}
          >
            {isBusy ? "Generuję..." : "Mock: wygeneruj szkic z wideo"}
          </button>

          {error && <p className="error-text">Error: {error}</p>}

          {lastResult && (
            <p className="info-text">
              Utworzono <strong>Game #{lastResult.gameId}</strong> i{" "}
              <strong>ListingDraft #{lastResult.listingDraftId}</strong>
            </p>
          )}
        </section>

        <section className="card">
          <h2>2. Listing preview (nasz HTML)</h2>
          {!preview && <p>Brak preview – wygeneruj coś z panelu obok.</p>}

          {preview && (
            <>
              <p>
                <strong>{preview.title}</strong>
                {preview.subtitle && <span> – {preview.subtitle}</span>}
              </p>
              <p>
                Marketplace: <code>{preview.marketplace}</code>
              </p>
              <p>
                Cena: {preview.price.amount.toFixed(2)} {preview.price.currency}
              </p>
              <p>
                Generated at:{" "}
                {new Date(preview.generatedAtUtc).toLocaleString()}
              </p>

              <h3>Opis HTML</h3>
              <div
                className="preview-html"
                dangerouslySetInnerHTML={{
                  __html: preview.descriptionHtml,
                }}
              />
            </>
          )}
        </section>

        <section className="card">
          <h2>3. Allegro payload (JSON)</h2>
          {!allegroPayload && (
            <p>Brak payloadu – wygeneruj coś z panelu po lewej.</p>
          )}

          {allegroPayload && (
            <>
              <p>
                <strong>Tytuł:</strong> {allegroPayload.title}
              </p>
              <p>
                <strong>Kategoria:</strong> {allegroPayload.categoryId}
              </p>
              <p>
                <strong>Język:</strong> {allegroPayload.language}
              </p>
              <p>
                <strong>Cena:</strong> {allegroPayload.price.amount.toFixed(2)}{" "}
                {allegroPayload.price.currency}
              </p>
              <p>
                <strong>Obrazy:</strong>{" "}
                {allegroPayload.imageUrls.length === 0
                  ? "brak"
                  : allegroPayload.imageUrls.join(", ")}
              </p>

              <h3>Surowy JSON</h3>
              <pre className="json-block">
                {JSON.stringify(allegroPayload, null, 2)}
              </pre>
            </>
          )}
        </section>
      </main>
    </div>
  );
}

export default App;
