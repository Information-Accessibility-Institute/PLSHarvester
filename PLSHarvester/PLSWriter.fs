namespace PLSHarvester

open System
open System.Xml
open System.Xml.Linq

module PlsWriter =

    /// Write a PLS dictionary from a map of Kanji Å® frequency
    let writePls (outputPath: string) (entries: Map<string,int>) =
        // PLS namespace
        let ns = XNamespace.Get("http://www.w3.org/2005/01/pronunciation-lexicon")

        // <lexicon> root element
        let lexicon =
            XElement(
                ns + "lexicon",
                XAttribute(XNamespace.Xml + "lang", "ja"),
                XAttribute("version", "1.0"),
                XAttribute("alphabet", "ipa")
            )

        // For each Kanji word, create <lexeme> element
        for (kanji, _) in Map.toSeq entries do
            let lexeme =
                XElement(
                    ns + "lexeme",
                    XElement(ns + "grapheme", kanji),
                    XElement(ns + "pronunciation", "")   // no reading
                )
            lexicon.Add(lexeme)

        // Save: create XDocument with declaration, then add root element
        let doc = XDocument(XDeclaration("1.0", "utf-8", null))
        doc.Add(lexicon)
        doc.Save(outputPath)

        printfn $"PLS dictionary written: {outputPath}"
