import { useEffect } from "react";
import { Trash2, X } from "lucide-react";

type Props = {
  title: string;
  text: string;
  confirmLabel: string;
  busy?: boolean;
  error?: string | null;
  onConfirm: () => void;
  onClose: () => void;
};

export default function ConfirmModal({ title, text, confirmLabel, busy, error, onConfirm, onClose }: Props) {
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape" && !busy) onClose();
    };
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [busy, onClose]);

  return (
    <div
      className="modal-backdrop"
      onMouseDown={(e) => {
        if (e.target === e.currentTarget && !busy) onClose();
      }}
    >
      <div className="modal modal-sm" role="alertdialog" aria-modal="true" aria-labelledby="confirm-title">
        <div className="modal-head">
          <div className="modal-icon danger"><Trash2 size={22} /></div>
          <div>
            <h3 id="confirm-title">{title}</h3>
            <p>این عملیات قابل بازگشت نیست</p>
          </div>
          <button type="button" className="icon-btn" onClick={onClose} disabled={busy} aria-label="بستن">
            <X size={18} />
          </button>
        </div>
        <div className="modal-body one-col">
          <p className="confirm-text">{text}</p>
          {error && <div className="form-error modal-full" role="alert">{error}</div>}
        </div>
        <div className="modal-foot">
          <button type="button" className="danger-btn" onClick={onConfirm} disabled={busy}>
            {busy ? "در حال حذف..." : confirmLabel}
          </button>
          <button type="button" className="secondary-btn" onClick={onClose} disabled={busy}>انصراف</button>
        </div>
      </div>
    </div>
  );
}
