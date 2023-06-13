using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScintillaNET.Demo.Utils {
	internal class SearchManager {

		public static ScintillaNET.Scintilla TextArea;
		public static TextBox SearchBox;

		public static string LastSearch = "";

		public static int LastSearchIndex;

		public static void Find(bool next, bool incremental) {
			bool first = LastSearch != SearchBox.Text;

			LastSearch = SearchBox.Text;
			if (LastSearch.Length > 0) {

				if (next) {

					// SEARCH FOR THE NEXT OCCURANCE

					// Search the document at the last search index
					TextArea.TargetStart = LastSearchIndex - 1;
					TextArea.TargetEnd = LastSearchIndex + (LastSearch.Length + 1);
					TextArea.SearchFlags = SearchFlags.None;

					// Search, and if not found..
					if (!incremental || TextArea.SearchInTarget(LastSearch) == -1) {

						// Search the document from the caret onwards
						TextArea.TargetStart = TextArea.CurrentPosition;
						TextArea.TargetEnd = TextArea.TextLength;
						TextArea.SearchFlags = SearchFlags.None;

						// Search, and if not found..
						if (TextArea.SearchInTarget(LastSearch) == -1) {

							// Search again from top
							TextArea.TargetStart = 0;
							TextArea.TargetEnd = TextArea.TextLength;

							// Search, and if not found..
							if (TextArea.SearchInTarget(LastSearch) == -1) {

								// clear selection and exit
								TextArea.ClearSelections();
								return;
							}
						}

					}

				} else {

					// SEARCH FOR THE PREVIOUS OCCURANCE

					// Search the document from the beginning to the caret
					int targetEnd = TextArea.CurrentPosition - LastSearch.Length;
                    TextArea.TargetStart = 0;
					TextArea.TargetEnd = targetEnd;
					TextArea.SearchFlags = SearchFlags.None;

					// Search, and if not found..
					int targetStart = -1;
					while(TextArea.SearchInTarget(LastSearch) >= 0)
					{
                        // Search again from the caret onwards
                        if (targetStart + LastSearch.Length == TextArea.CurrentPosition)
                            break;
                        targetStart = TextArea.TargetStart;

						TextArea.TargetStart = targetStart + LastSearch.Length;
						TextArea.TargetEnd = targetEnd;
                    }

					if (targetStart >= 0)
                    {
						TextArea.TargetStart = targetStart;
						TextArea.TargetEnd = targetStart + LastSearch.Length;
                    }
					else
					{
                        // clear selection and exit
                        TextArea.ClearSelections();
                        return;
                    }
				}

				// Select the occurance
				LastSearchIndex = TextArea.TargetStart;
				TextArea.SetSelection(TextArea.TargetEnd, TextArea.TargetStart);
				TextArea.ScrollCaret();

			}

			SearchBox.Focus();
		}


	}
}
